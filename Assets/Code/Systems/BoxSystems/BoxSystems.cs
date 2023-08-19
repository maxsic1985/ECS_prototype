using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;

namespace MSuhininTestovoe.B2B
{
    public class BoxSystems
    {
        public BoxSystems(EcsSystems systems)
        {
            systems
                .Add(new BoxLoadSystem())
                .Add(new BoxInitSystem())
                .Add(new BoxBuildSystem());
            // .Add(new BackgroundMoveSystem())
            // .Add(new BackgroundCheckPositionSystem());
        }
    }
}