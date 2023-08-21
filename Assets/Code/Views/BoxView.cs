using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    public sealed class BoxView : BaseView
    {
        [SerializeField] private Transform _transformView;
        [SerializeField] [Range(0.1f, 2)] private float _speedPingPong = 0.5f;

        
        private void OnEnable()
        {
            _speedPingPong = new System.Random().Next(1, 10) * 0.1f;
            transform.position = new Vector2(new System.Random().Next(0, 5), 0);
        }

        
        private void Update()
        {
            float pingPong = Mathf.PingPong(Time.time * _speedPingPong, 1);
            var a = new Vector2(0, -3);
            var b = new Vector2(0, 3);
            var aa = Vector2.Lerp(a, b, pingPong);
            transform.position = new Vector2(transform.position.x, aa.y);
        }
    }
}