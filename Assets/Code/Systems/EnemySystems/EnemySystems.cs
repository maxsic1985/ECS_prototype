using LeoEcsPhysics;
using Leopotam.EcsLite;

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
                .DelHerePhysics();
            //  .Add(new EnemyAtackSystem());

        }
    }
}