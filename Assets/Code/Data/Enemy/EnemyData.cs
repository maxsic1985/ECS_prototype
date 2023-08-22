using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MSuhininTestovoe.B2B
{


    [CreateAssetMenu(fileName = nameof(CameraLoadData),
        menuName = EditorMenuConstants.CREATE_DATA_MENU_NAME + nameof(EnemyData))]
    public class EnemyData : ScriptableObject
    {
        [Header("Prefabs:")]
        public AssetReferenceGameObject EnemyPrefab;
        public int CountForInstantiate;
        public int Lives;
        [Header("Positions:")]
        public Vector3 [] StartPositions;
        [Header("Rotations:")]
        public Vector3 [] StartRotation;

        [Header("SecurityZone:")] public float SecurityZoneDistance;
    }
}

