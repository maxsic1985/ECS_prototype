using UnityEngine;
using UnityEngine.Rendering;


namespace MSuhininTestovoe.B2B
{
    public sealed class PostProcessingView : BaseView
    {
        [SerializeField] private Volume _volume;

        public Volume Volume => _volume;
    }
}