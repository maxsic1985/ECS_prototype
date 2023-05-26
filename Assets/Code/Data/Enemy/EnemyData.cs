using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MSuhininTestovoe.B2B
{


    [CreateAssetMenu(fileName = nameof(CameraLoadData),
        menuName = EditorMenuConstants.ENEMY + nameof(EnemyData))]
    public class EnemyData : ScriptableObject
    {
        [Header("Prefabs:")]
        public AssetReferenceGameObject EnemyPrefab;
        [Header("Positions:")]
        public Vector3 [] StartPositions;
        [Header("Rotations:")]
        public Vector3 [] StartRotation;
    }
}

