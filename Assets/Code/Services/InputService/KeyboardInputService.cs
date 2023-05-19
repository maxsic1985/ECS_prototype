using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class KeyboardInputService : IInputService
    {
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }

        public void Update()
        {
            Horizontal = Input.GetKeyDown(KeyCode.A) 
                ? -1f 
                : Input.GetKeyDown(KeyCode.D) ? 1 : 0;
            
            Vertical = Input.GetKeyDown(KeyCode.W) 
                ? 1 
                : Input.GetKeyDown(KeyCode.S) ? -1 : 0;
        }
    }
}
