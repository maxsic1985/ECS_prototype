using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public sealed class PlayerBoxColliderView : BaseView

    {
        [SerializeField] private BoxCollider _boxCollider;

        public BoxCollider BoxCollider => _boxCollider;
    }
}