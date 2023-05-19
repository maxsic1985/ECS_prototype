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
                .Add(new LoadDataByNameSystem())
              //  .Add(new RespawnPlatformSystem())
                .Add(new OnGroundSystem());
              //  .Add(new InstantiatePickableObjectsSystem());
          
            new CommonSystems(systems);
            new PlayerSystems(systems);
            new CameraSystems(systems);
            new MenuSystems(systems);
            //   new PostProcessingSystems(systems);
           
            //Use Gravity 
            //Sounds
            new SoundSystems(systems);
           // new DeathSystems(systems);
            //Trigger
            systems.Add(new TriggerSystem());
        }
    }
}