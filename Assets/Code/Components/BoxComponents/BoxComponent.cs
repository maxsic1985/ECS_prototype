using System.Collections.Generic;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public struct BoxComponent
    {
        public float Speed;
        public int StartBoxCount;
        public Transform ParentTransform;
        public Vector2 ReturnToPoolPoint;
        public float[] SpawnBoxPoint;
        public float BoxCountOnSceene;
    }
}