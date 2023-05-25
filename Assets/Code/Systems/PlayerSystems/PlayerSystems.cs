using Leopotam.EcsLite;

namespace MSuhininTestovoe.B2B
{
    public class PlayerSystems
    {
        public PlayerSystems(EcsSystems systems)
        {
            systems
                .Add(new PlayerSystem())
                .Add(new PlayerInitSystem())
            .Add(new PlayerBuildSystem())
            .Add(new PlayerInputSystem())
            .Add(new PlayerMoveSystem());

        }
    }
}