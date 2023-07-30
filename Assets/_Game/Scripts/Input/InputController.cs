using System;
using UnityEngine;

namespace PanteonDemo
{
    public enum InputType
    {
        MouseAndKeyboard,
        MobileFinger,
        MobileJoystick
    }
    
    public enum TouchState
    {
        Begin,
        End
    }
    
    /// <summary>
    /// Event is triggered when tapping
    /// </summary>
    public struct TapEvent
    {
        public TouchState NewState { get; }
        public Vector3 Position { get; }
        
        public TapEvent(TouchState newState, Vector3 position)
        {
            NewState = newState;
            Position = position;
        }
    }

    public class InputController : Singleton<InputController>
    {
        [SerializeField] private InputType _inputType;
        
        private Vector3 _firstTapPosition;
        private Vector3 _endTapPosition;
        private Vector3 _swipeDirection;

        private void Update()
        {
            switch (_inputType)
            {
                case InputType.MouseAndKeyboard:
                    DetectMouseInput(0);
                    break;
                case InputType.MobileFinger:
                    DetectTouchInput();
                    break;
                case InputType.MobileJoystick:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DetectTouchInput()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _firstTapPosition = touch.position;
                }
                
                else if (touch.phase == TouchPhase.Ended)
                {
                    _endTapPosition = touch.position;
                }
            }
        }
        
        
        private void DetectMouseInput(uint mouseButtonData)
        {
            if (mouseButtonData <= 3)
            {
                int mouseButton = (int) mouseButtonData;

                if (Input.GetMouseButtonDown(mouseButton))
                {
                    _firstTapPosition = Input.mousePosition;
                    print("mouse down position: " + _firstTapPosition);
                }
                
                else if (Input.GetMouseButtonUp(mouseButton))
                {
                    _endTapPosition = Input.mousePosition;
                    print("mouse up position: " + _endTapPosition);
                }
            }
        }
    }
}
