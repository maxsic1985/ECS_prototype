using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class PauseMenuHandBuildSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PrefabComponent> _prefabPool;
        private EcsPool<CloseBtnCommand> _closMenuPool;


        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<CloseBtnCommand>().Inc<PrefabComponent>().End();
            _prefabPool = world.GetPool<PrefabComponent>();
            _closMenuPool = world.GetPool<CloseBtnCommand>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var prefabComponent = ref _prefabPool.Get(entity);
                var gameObject = Object.Instantiate(prefabComponent.Value);
                var canvas = GameObject.FindObjectOfType<Canvas>();
                gameObject.transform.parent = canvas.transform;
              //  gameObject.transform.localPosition = Vector3.zero;
                ref var menu = ref _closMenuPool.Get(entity);
                menu.MenuValue = gameObject.GetComponent<TransformView>().gameObject;
               _prefabPool.Del(entity);
            }
        }
    }
}