using Leopotam.EcsLite;

namespace MSuhininTestovoe.B2B
{
    public class BoxSystems
    {
        public BoxSystems(EcsSystems systems)
        {
            systems
                .Add(new BoxLoadSystem())
                .Add(new BoxInitSystem())
                .Add(new BoxBuildSystem())
                .Add(new BoxRespawnSystem())
                .Add(new BoxSecurityZoneSystem())
              .Add(new BoxCheckPositionSystem());
        }
    }
}