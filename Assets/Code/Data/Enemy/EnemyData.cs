using UnityEngine;



namespace MSuhininTestovoe.B2B
{


    [CreateAssetMenu(fileName = nameof(CameraLoadData),
        menuName = EditorMenuConstants.CREATE_DATA_MENU_NAME + nameof(EnemyData))]
    public class EnemyData : ScriptableObject
    {
        [Header("Positions:")]
        public Vector3  StartPositions;
        [Header("Rotations:")]
        public Vector3  StartRotation;

        [Header("SecurityZone:")] public float SecurityZoneDistance;
    }
}

