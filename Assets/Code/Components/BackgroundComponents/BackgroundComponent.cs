using System.Collections.Generic;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public struct BackgroundComponent
    {
        public float Speed;
        public int StartPlatformCount;
        public Transform ParentTransform;
        public Vector2 ReturnToPoolPoint;
        public Vector2[] SpawnPlatformPoint;
        public float PlatformLength;
    }
}