using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public sealed class PlayerRigidBodyView: BaseView
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        public Rigidbody2D Rigidbody => _rigidbody;
    }
}