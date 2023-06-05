using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;

namespace MSuhininTestovoe.B2B
{
    public class EnemySystems
    {
        public EnemySystems(EcsSystems systems)
        {
            systems
                .Add(new EnemySystem())
                .Add(new EnemyInitSystem())
                .Add(new EnemyBuildSystem())
                .Add(new EnemySecurityZoneSystem())
                .Add(new EnemyAtackSystem())
                .DelHerePhysics();
        }
    }
}