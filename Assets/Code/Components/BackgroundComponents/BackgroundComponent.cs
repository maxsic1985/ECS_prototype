using System.Collections.Generic;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public struct BackgroundComponent
    {
        public float Speed;
        public float AccelerationInterval;
        public float AccelerationValue;
        public int StartPlatformCount;
        public int PlatformsBeforePlayer;
        public Vector3 ReturnToPoolPoint;
        public Vector3 SpawnPlatformPoint;
        public float PlatformLength;
        public Queue<BackgroundView> Platforms;
        public GameObjectsTypeId UsingPlatform;
    }
}