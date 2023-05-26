using Leopotam.EcsLite;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public class PauseMenuBuildSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<PrefabComponent> _prefabPool;
        private EcsPool<IsPauseMenu> _showMenuPool;


        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<IsPauseMenu>().Inc<PrefabComponent>().End();
            _prefabPool = world.GetPool<PrefabComponent>();
            _showMenuPool = world.GetPool<IsPauseMenu>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var prefabComponent = ref _prefabPool.Get(entity);
                var gameObject = Object.Instantiate(prefabComponent.Value);
                var canvas = GameObject.FindObjectOfType<Canvas>();
                gameObject.transform.parent = canvas.transform;
                gameObject.transform.localPosition = Vector3.zero;
                ref var menu = ref _showMenuPool.Get(entity);
                menu.MenuValue = gameObject.GetComponent<TransformView>().gameObject;
                menu.MenuValue.SetActive(false);
                _prefabPool.Del(entity);
            }
        }
    }
}