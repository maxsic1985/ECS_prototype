using System.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public interface IPoolService
    {
        Task Initialize();
        GameObject Get(GameObjectsTypeId gameObjectsTypeId,EcsWorld world);
        void Add(GameObjectsTypeId type, GameObject pooledObject, int capacity);
        void Clear();
        void Clear(GameObjectsTypeId gameObjectsTypeId);
        void Return(GameObject gameObject);
        int Count { get; }
    }
}