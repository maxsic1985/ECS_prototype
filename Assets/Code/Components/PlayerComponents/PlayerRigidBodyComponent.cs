using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public struct PlayerRigidBodyComponent
    {
        public Rigidbody2D PlayerRigidbody;
        public float AngularDrag;
        public float GravityScale;
        public float Mass;
        public bool AutoMass;

        public void SetRigidBodyParametrs()
        {
            PlayerRigidbody.angularDrag = AngularDrag;
            PlayerRigidbody.gravityScale = GravityScale;
            PlayerRigidbody.useAutoMass = AutoMass;
            PlayerRigidbody.mass = Mass;
        }
    }
}