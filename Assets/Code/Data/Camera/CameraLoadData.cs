using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MSuhininTestovoe.B2B
{


    [CreateAssetMenu(fileName = nameof(CameraLoadData),
        menuName = EditorMenuConstants.CREATE_DATA_MENU_NAME + nameof(CameraLoadData))]
    public class CameraLoadData : ScriptableObject
    {
        [Header("Prefabs:")]
        public AssetReferenceGameObject Camera;
        [Header("Positions:")]
        public Vector3 StartPosition; 
        [Header("Roation:")]
        public Vector3 StartRotation;
        [Header("OffSet:")]
        public Vector3 OffSetPosition;
        [Range(0f, 1f)]
        public float CameraSmoothness;
    }
}

