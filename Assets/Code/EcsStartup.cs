using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public sealed class EcsStartup : MonoBehaviour
    {
        private EcsSystems _systems;
        private bool _hasInitCompleted;
        [SerializeField] EcsUguiEmitter uguiEmitter;

        private async void Start()
        {
            SharedData shared = new();
            await shared.Init();

            IPoolService poolService = new PoolService();
            await poolService.Initialize();

            IPatternService patternService = new PatternService();
            await patternService.Initialize();

            var world = new EcsWorld();
            _systems = new EcsSystems(world,shared);

            new InitializeAllSystem(_systems, poolService);

            _systems
                .AddWorld (new EcsWorld (), WorldsNamesConstants.EVENTS)
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(WorldsNamesConstants.EVENTS))

#endif
                .InjectUgui (uguiEmitter, WorldsNamesConstants.EVENTS)
                .Init();
            _hasInitCompleted = true;
        }

        private void Update()
        {
            if (_hasInitCompleted)
                _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                foreach (var worlds in  _systems.GetAllNamedWorlds())
                {
                    worlds.Value.Destroy();
                }
                _systems.GetWorld().Destroy();
                _systems = null;
            }
        }
    }
}