using Leopotam.EcsLite;



namespace MSuhininTestovoe.B2B
{
    internal class CommonSystems
    {
        public CommonSystems(EcsSystems systems)
        {
            systems.Add(new RotateSystem());
            systems.Add(new TestRotateSystem());
            systems.Add(new TransformMovingSystem());
            systems.Add(new SynchronizeTransformAndPositionSystem());
            systems.Add(new RestartSystem());
            systems.Add(new TimerRunSystem());
        }
    }
}