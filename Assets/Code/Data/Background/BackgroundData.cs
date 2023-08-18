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
        public AssetReferenceGameObject Grid;
        public float Speed;
       [HideInInspector]
        public int StartPlatformCount=>StartPlatformPosition.Length;
        public Vector2[] StartPlatformPosition;
        public int PlatformsBeforePlayer => StartPlatformCount - 1;
    }
}

