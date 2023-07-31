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
    public struct InputEvent
    {
        public TouchState State { get; }
        public Vector3 Position { get; }

        public InputEvent(TouchState state, Vector3 position)
        {
            State = state;
            Position = position;
        }
    }

    public class InputController : Singleton<InputController>
    {
        [SerializeField] private InputType _inputType;

        private Vector3 _firstPosition;
        private Vector3 _endPosition;
        private Vector3 _currentPosition;
        private Vector3 _swipeVector;
        private Vector3 _swipeDirection;
        private float _swipeLength;

        public Vector3 CurrentPosition => _currentPosition;

        public Vector3 SwipeVector => _swipeVector;

        public Vector3 SwipeDirection => _swipeDirection;

        public float SwipeLength => _swipeLength;

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
                Vector3 pos = touch.position;

                if (touch.phase == TouchPhase.Began)
                {
                    _firstPosition = pos;
                    EventManager.TriggerEvent(new InputEvent(TouchState.Begin, _firstPosition));
                }

                else if (touch.phase == TouchPhase.Ended)
                {
                    _endPosition = pos;
                    EventManager.TriggerEvent(new InputEvent(TouchState.End, _endPosition));
                }

                _currentPosition = pos;
                _swipeVector = _currentPosition - _firstPosition;
                _swipeLength = Vector3.Distance(_currentPosition, _firstPosition);
                _swipeDirection = _swipeVector.normalized;
            }
        }


        private void DetectMouseInput(uint mouseButtonData)
        {
            if (mouseButtonData <= 3)
            {
                int mouseButton = (int) mouseButtonData;

                if (Input.GetMouseButton(mouseButton))
                {
                    if (Input.GetMouseButtonDown(mouseButton))
                    {
                        _firstPosition = Input.mousePosition;
                        EventManager.TriggerEvent(new InputEvent(TouchState.Begin, _firstPosition));
                    }

                    else if (Input.GetMouseButtonUp(mouseButton))
                    {
                        _endPosition = Input.mousePosition;
                        EventManager.TriggerEvent(new InputEvent(TouchState.End, _endPosition));
                    }

                    _currentPosition = Input.mousePosition;
                    _swipeVector = _currentPosition - _firstPosition;
                    _swipeLength = Vector3.Distance(_currentPosition, _firstPosition);
                    _swipeDirection = _swipeVector.normalized;
                }
            }
        }
    }
}