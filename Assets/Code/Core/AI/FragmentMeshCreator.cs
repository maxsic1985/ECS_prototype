using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FragmentMeshCreator : MeshCreatorBase
{
    public virtual void Create(float angle, float distance, float step = 10f)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        Vector3 right = ViewUtility.GetRotation(Vector3.forward, angle) * distance;
        Vector3 left = ViewUtility.GetRotation(Vector3.forward, angle) * distance;
        Vector3 from = left;

        vertices.Add(Vector3.zero);
        vertices.Add(from);
        uvs.Add(Vector2.one * 0.5f);
        uvs.Add(Vector2.one);
        int triangleIdx = 3;

        for (float angleStep = -angle; angleStep < angle; angleStep += step)
        {
            Vector3 to = ViewUtility.GetRotation(Vector3.forward, angleStep) * distance;
            from = to;
            vertices.Add(from);
            uvs.Add(Vector2.one);
            triangles.Add(triangleIdx - 1);
            triangles.Add(triangleIdx);
            triangles.Add(0);

            triangleIdx++;
        }
        vertices.Add(right);

        uvs.Add(Vector2.one);

        Mesh mesh = new Mesh();
        mesh.indexFormat =IndexFormat.UInt32;
        mesh.name = "FragmentArea";
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        _myMeshFilter.mesh = mesh;
        

        //_meshCollider = new MeshCollider();
        _meshCollider.sharedMesh = mesh;
        _meshCollider.sharedMesh.vertices = mesh.vertices;
        _meshCollider.sharedMesh.triangles = mesh.triangles;


        _meshCollider.convex = true;
       _meshCollider.isTrigger = true;
    }

    private void Start()
    {
        Create(45,1);
    }
}
public static class ViewUtility
{
    public static Vector3 GetRotation(Vector3 forward, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector3 result = new Vector3(forward.x * Mathf.Cos(rad) + forward.z * Mathf.Sin(rad), 0,
                                        forward.z * Mathf.Cos(rad) - forward.x * Mathf.Sin(rad));
        return result;
    }
}





[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MeshCreatorBase:MonoBehaviour
{
   public MeshFilter _myMeshFilter;
    public MeshCollider _meshCollider;

    private void OnEnable()
    {
        _myMeshFilter = GetComponent<MeshFilter>();
       _meshCollider = GetComponent<MeshCollider>();

    }
}