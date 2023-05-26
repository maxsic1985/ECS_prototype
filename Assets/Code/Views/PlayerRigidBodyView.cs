using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public sealed class PlayerRigidBodyView: BaseView
    {
        [SerializeField] private Rigidbody _rigidbody;

        public Rigidbody Rigidbody => _rigidbody;
    }
}