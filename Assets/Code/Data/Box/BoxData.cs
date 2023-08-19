using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MSuhininTestovoe.B2B
{
    [CreateAssetMenu(fileName = nameof(BackgroundData),
        menuName = EditorMenuConstants.CREATE_DATA_MENU_NAME + nameof(BoxData))]
    public class BoxData : ScriptableObject
    {
      //  [Header("Prefabs:")]
     //   public AssetReferenceGameObject Value;
        public float Speed;
        [HideInInspector] public int StartBoxCount;
        public float[] StartBoxPoints;
    }
}