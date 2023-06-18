using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    public sealed class PlayerHealthView : BaseView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
     

        public SpriteRenderer Value => _spriteRenderer;
       
    }
}