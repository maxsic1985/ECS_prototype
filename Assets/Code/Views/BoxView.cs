using System;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public sealed class BoxView : BaseView
    {
        [SerializeField] private Transform _transformView;

        private void Update()
        {
            float pingPong = Mathf.PingPong(Time.time * 1, 1);
            var a = new Vector2(0, -3);
            var b = new Vector2(0, 3);
         var aa = Vector2.Lerp(a, b, pingPong);
         transform.position = new Vector2(transform.position.x, aa.y);
        }
    }
}