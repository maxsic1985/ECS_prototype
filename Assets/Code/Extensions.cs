using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public static class Extensions
    {
      
        public static T GetorAddPool<T>(IEcsSystems ecsSystem, int entity) where T : struct
        {
            if (ecsSystem.GetWorld().GetPool<T>().Has(entity))
            {
               return ecsSystem.GetWorld().GetPool<T>().Get(entity);
            }
            else
            {
                ref var component = ref ecsSystem
                    .GetWorld()
                    .GetPool<T>().Add(entity);
                return component;
            }
            
        }
        
        public static int[] GetUniqeRandomArray(int size, int Min, int Max)
        {
            int[] UniqueArray = new int[size];
            var rnd = new System.Random();
            int Random;

            for (int i = 0; i < size; i++)
            {
                Random = rnd.Next(Min, Max);

                for (int j = i; j >= 0; j--)
                {
                    if (UniqueArray[j] == Random)
                    {
                        Random = rnd.Next(Min, Max);
                        j = i;
                    }
                }

                UniqueArray[i] = Random;
            }

            return UniqueArray;
        }
    }
    
    
    
    
    
}