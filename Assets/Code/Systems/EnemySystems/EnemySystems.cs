﻿using LeoEcsPhysics;
using Leopotam.EcsLite;



namespace MSuhininTestovoe.B2B
{
    public class EnemySystems
    {
        public EnemySystems(EcsSystems systems)
        {
            systems
                .Add(new EnemyLoadSystem())
                .Add(new EnemyInitSystem())
                .Add(new EnemyMoveSystem())
                .Add(new EnemySecurityZoneSystem())
                .Add(new EnemyCheckPosiionSystem())
                .Add(new EnemyRayCastSystem())
                .Add(new EnemyRespawnSystem())
                .DelHerePhysics();
        }
    }
}