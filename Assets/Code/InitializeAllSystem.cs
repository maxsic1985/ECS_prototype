using LeoEcsPhysics;
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
           new CameraSystems(systems);
           new BackgroundSystems(systems);
           //new EnemySystems(systems);
           //new MenuSystems(systems);
           // new DeathSystems(systems);


           //   new SoundSystems(systems);
        }
    }
}