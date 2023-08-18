using System.Collections.Generic;
using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public struct IsPlatformComponent
    {
        public List<GameObject> PillarLamps;
        public List<GameObject> WallLamps;
        public List<GameObject> PickableObjects;

        public void Clear()
        {
            PillarLamps.Clear();
            WallLamps.Clear();
            PickableObjects.Clear();
        }
    }
}