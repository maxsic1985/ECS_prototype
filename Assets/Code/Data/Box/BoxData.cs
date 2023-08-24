using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    [CreateAssetMenu(fileName = nameof(BackgroundData),
        menuName = EditorMenuConstants.CREATE_DATA_MENU_NAME + nameof(BoxData))]
    public class BoxData : ScriptableObject
    {
        public float Speed;

        public int MinSpeedBox;
        public int MaxSpeedBox;

        public int UpperPoint;
        public int DownerPoint;

        public int SpawnHorisontalPoint;

    }
}