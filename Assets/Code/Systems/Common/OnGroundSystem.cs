using Leopotam.EcsLite;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class OnGroundSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const int GroundLayerMask = 1 << 6;
        
        private EcsFilter _filter;
        private EcsPool<TransformComponent> _transformComponentPool;
        private EcsPool<IsOnGroundComponent> _isOnGroundComponentPool;
        private SkinnedMeshRenderer _skinnedMeshRenderer;


        private readonly RaycastHit[] _results;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<GravityComponent>().Inc<TransformComponent>().End();
            _transformComponentPool = world.GetPool<TransformComponent>();
            _isOnGroundComponentPool = world.GetPool<IsOnGroundComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                if (_skinnedMeshRenderer is null)
                {
                    ref TransformComponent transformComponent = ref _transformComponentPool.Get(entity);
                    
                    if (transformComponent.Value is null) return;
                    
                    _skinnedMeshRenderer =
                        transformComponent.Value.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                    return;
                }
                ref TransformComponent trComponent  = ref _transformComponentPool.Get(entity);
                
                if (Contact(ref trComponent) && !_isOnGroundComponentPool.Has(entity))
                    _isOnGroundComponentPool.Add(entity);
                
                if (!Contact(ref trComponent) && _isOnGroundComponentPool.Has(entity))
                    _isOnGroundComponentPool.Del(entity);
            }
        }
        
        private bool Contact(ref TransformComponent transformComponent)
        {
            return Physics.CheckSphere(transformComponent.Value.position, .2f, GroundLayerMask);
        }
        
    }
}