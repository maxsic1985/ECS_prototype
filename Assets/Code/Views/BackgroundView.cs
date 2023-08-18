using System;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class BackgroundView : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private PatternGroups _patternGroup;
        public event Action Activated;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 StartPointWorldPosition => _startPoint.position;
        public Vector3 EndPointWorldPosition => _endPoint.position;

        public float GetPlatformLenght()
        {
            return Vector3.Distance(_startPoint.position, _endPoint.position);
        }

        private void OnEnable()
        {
            Activated?.Invoke();
        }
    }
}