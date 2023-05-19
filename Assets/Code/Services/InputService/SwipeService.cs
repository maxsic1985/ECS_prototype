using UnityEngine;

namespace MSuhininTestovoe.B2B
{
    public class SwipeService : IInputService
    {
        private const float MINSwipeLength = 200f;
        private const int Left = -1; 
        private const int Right = 1;
        private const int Up = 1;
        private const int Down = -1;
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
    
        private readonly bool _isMobile;
        
        private Touch _touch;
        private bool _canSwipe;
        private Vector2 _firstPressPos;
        private Vector2 _secondPressPos;
        private Vector2 _currentSwipe;
        private Vector2 _tapPosition;
        private Vector2 _swipeDelta;
        private bool _isSwiping;
    
        public SwipeService() => 
            _isMobile = Application.isMobilePlatform;

        public void Update()
        {
            Horizontal = 0;
            Vertical = 0;
            
            if (!_isMobile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _isSwiping = true;
                    _tapPosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    ResetSwipe();
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        _isSwiping = true;
                        _tapPosition = Input.GetTouch(0).position;
                    }
                    else if (Input.GetTouch(0).phase is TouchPhase.Canceled or TouchPhase.Ended)
                    {
                        ResetSwipe();
                    }
                }
            }
        
            CheckSwipe();
        }

        private void CheckSwipe()
        {
            _swipeDelta = Vector2.zero;
            if (_isSwiping)
            {
                if (!_isMobile && Input.GetMouseButton(0))
                {
                    _swipeDelta = (Vector2) Input.mousePosition - _tapPosition;
                }
                else if (Input.touchCount > 0)
                {
                    _swipeDelta = Input.GetTouch(0).position - _tapPosition;
                }
            }

            if (_swipeDelta.magnitude > MINSwipeLength)
            {
                if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
                    Horizontal = _swipeDelta.x > 0 ? Right : Left;
                else
                    Vertical = _swipeDelta.y > 0 ? Up : Down;
            
                ResetSwipe();
            }
        }

        private void ResetSwipe()
        {
            _tapPosition = Vector2.zero;
            _swipeDelta = Vector2.zero;
            _isSwiping = false;
        }
    
    }
}
