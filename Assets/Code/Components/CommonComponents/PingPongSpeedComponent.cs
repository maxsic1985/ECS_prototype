namespace MSuhininTestovoe.B2B
{
    public struct PingPongSpeedComponent
    {
        public int MinValue;
        public int MaxValue;
        public float CurrentValue;

        
        public float GetRandomSpeed  =>new System.Random().Next(MinValue, MaxValue) * 0.1f;
    }
}