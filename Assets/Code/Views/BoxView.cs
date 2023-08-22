using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public sealed class BoxView : BaseView
    {
        public int MinSpeedPingPong { get; internal set; } = 1;
        public int MaxSpeedPingPong { get; internal set; } = 5;
        public float PingPongUpPoint { get; internal set; } = 3f;
        public float PingPongDownPoint { get; internal set; } = 3f;

        private float _speedPingPong = 1;


        private void OnEnable()
        {
            _speedPingPong = new System.Random().Next(MinSpeedPingPong, MaxSpeedPingPong) * 0.1f;
            transform.position = new Vector2(new System.Random().Next(0, 5), 0);
        }


        private void Update()
        {
            float pingPong = Mathf.PingPong(Time.time * _speedPingPong, 1);
            var downposition = new Vector2(0, PingPongDownPoint);
            var upPosition = new Vector2(0, PingPongUpPoint);
            var direction = Vector2.Lerp(downposition, upPosition, pingPong);
            transform.position = new Vector2(transform.position.x, direction.y);
        }
    }
}