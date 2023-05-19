using Leopotam.EcsLite;


namespace HalfDiggers.Runner
{
    public class DeathSystems
    {
        public DeathSystems(EcsSystems systems)
        {
            systems
                .Add(new DEBUG_DeathSystem())
                .Add(new DeathSystem());
        }
    }
}