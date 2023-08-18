using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MSuhininTestovoe.B2B
{


    [CreateAssetMenu(fileName = nameof(BackgroundData),
        menuName = EditorMenuConstants.CREATE_DATA_MENU_NAME + nameof(BackgroundData))]
    public class BackgroundData : ScriptableObject
    {
       // [Header("Prefabs:")]
        public AssetReferenceGameObject Value;
        public float Speed;
        public float AccelerationInterval;
        public float AccelerationValue;
        public int StartPlatformCount;
        public GameObjectsTypeId UsingPlatform;
        public int PlatformsBeforePlayer => StartPlatformCount - 1;
    }
}

