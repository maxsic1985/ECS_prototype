using UnityEngine;
using UnityEngine.AddressableAssets;


namespace MSuhininTestovoe.B2B
{
    [CreateAssetMenu(fileName = nameof(UIPauseMenuBtnData),
        menuName = EditorMenuConstants.CREATE_DATA_MENU_NAME + nameof(UIPauseMenuBtnData))]
    public class UIPauseMenuBtnData : ScriptableObject
    {
        public AssetReferenceGameObject BtnShowMenu;
    }
}