using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public static class Extensions
    {
      
        public static void AddPool<T>(IEcsSystems ecsSystem, int entity) where T : struct
        {
            if (ecsSystem.GetWorld().GetPool<T>().Has(entity))
            {
               return;
            }
            else
            {
                ref var component = ref ecsSystem
                    .GetWorld()
                    .GetPool<T>().Add(entity);
            }
        }
    }
}