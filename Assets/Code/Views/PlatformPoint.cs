using UnityEngine;

namespace GameObjectsScripts.Platform
{
    [ExecuteInEditMode]
    public class PlatformPoint : MonoBehaviour
    {
        [SerializeField] private Color _color;
        [SerializeField] [Range(0, 1f)] private float _radius; 
        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
}