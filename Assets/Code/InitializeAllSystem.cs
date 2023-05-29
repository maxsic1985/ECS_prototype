using Leopotam.EcsLite;

namespace MSuhininTestovoe.B2B
{
    internal class InitializeAllSystem
    {
        public InitializeAllSystem(EcsSystems systems, IPoolService poolService)
        {
            systems
                .Add(new InitializeServiceSystem(poolService))
                .Add(new LoadPrefabSystem())
                .Add(new LoadDataByNameSystem());

            new CommonSystems(systems);
            new PlayerSystems(systems);
            new EnemySystems(systems);
            
         //   new CameraSystems(systems);
        //    new MenuSystems(systems);
         //   new SoundSystems(systems);
        }
    }
}