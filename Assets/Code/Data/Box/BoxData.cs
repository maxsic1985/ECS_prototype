using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MSuhininTestovoe.B2B
{
    [CreateAssetMenu(fileName = nameof(BackgroundData),
        menuName = EditorMenuConstants.CREATE_DATA_MENU_NAME + nameof(BoxData))]
    public class BoxData : ScriptableObject
    {
        public float Speed;
        public int StartBoxCount;
        public float[] StartBoxPoints;

        public int MinSpeedBox;
        public int MaxSpeedBox;

        public int UpPoint;
        public int DownPoint;
    }
}