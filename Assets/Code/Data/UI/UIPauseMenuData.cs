using UnityEngine;
using UnityEngine.AddressableAssets;


namespace MSuhininTestovoe.B2B
{
    [CreateAssetMenu(fileName = nameof(UIPauseMenuData),
        menuName = EditorMenuConstants.CREATE_DATA_MENU_NAME + nameof(UIPauseMenuData))]
    public class UIPauseMenuData : ScriptableObject
    {
        [Header("Menu")] 
        public AssetReferenceGameObject Menu; 

    }
}