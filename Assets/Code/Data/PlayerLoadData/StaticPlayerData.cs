using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MSuhininTestovoe.B2B
{
    [CreateAssetMenu(fileName = nameof(StaticPlayerData),
        menuName = EditorMenuConstants.CREATE_PLAYER + nameof(StaticPlayerData))]
    public class StaticPlayerData : ScriptableObject
    {
        [Header("Prefabs:")]
        public AssetReferenceGameObject Player;

        [Header("Position:")] public Vector3 StartPosition;
        [Header("Player moving data:")] 
        public float MoveDistance;
        public float JumpHeigth;
        public float MoveSpeed;
        public float JumpSpeed;
        
    }
}