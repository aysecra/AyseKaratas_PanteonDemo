using StrategyDemo.Controller;
using StrategyDemo.Enum;
using StrategyDemo.Event;
using StrategyDemo.Interfaces;
using StrategyDemo.Logic;
using StrategyDemo.Manager;
using StrategyDemo.SO;
using UnityEngine;

namespace StrategyDemo.Component
{
    public class LeftClickControl : Singleton<LeftClickControl>
        , IUpdateListener
        , EventListener<InputEvent>
        , EventListener<PlacementEvent>
    {
        [SerializeField] private UpdateManagementSO updateSO;

        private Camera _mainCamera;
        private bool _isTouch;
        IDragable _spawnedDragableObject;

        public bool IsClickable { get; set; }

        public IDragable SpawnedDragableObject
        {
            get => _spawnedDragableObject;
            set => _spawnedDragableObject = value;
        }

        private void Start()
        {
            IsClickable = true;
            _mainCamera = Camera.main;
            DraggingObjectLogic.IsTouchStart = false;
        }

        public void ManagedUpdate()
        {
            if (IsClickable && _isTouch)
            {
                if (!ReferenceEquals(_spawnedDragableObject, null))
                    DraggingObjectLogic.DetectAndDragObject(_mainCamera, _spawnedDragableObject);
                else DetectingAndOpenInfoArea.OpenInfo(_mainCamera);
                
                if (ReferenceEquals(RightClickControl.Instance.MovableObjectScript, null))
                {
                    Vector2 currentMousePoint = InputManager.Instance.CurrentPosition;
                    Vector3 origin = _mainCamera.ScreenToWorldPoint(currentMousePoint);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero);

                    foreach (RaycastHit2D hit in hits)
                    {
                        Transform hitTransform = hit.transform;
                        {
                            if (hitTransform.TryGetComponent(out IMovableWithPath movableObject))
                            {
                                RightClickControl.Instance.MovableObject = hitTransform.gameObject;
                                RightClickControl.Instance.MovableObjectScript = movableObject;
                                movableObject.SetIsSelectedObject(true);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            updateSO.AddListener(this);
            EventManager.EventStartListening<InputEvent>(this);
            EventManager.EventStartListening<PlacementEvent>(this);
        }

        private void OnDisable()
        {
            updateSO.RemoveListener(this);
            EventManager.EventStopListening<InputEvent>(this);
            EventManager.EventStopListening<PlacementEvent>(this);
        }

        public void OnEventTrigger(InputEvent currentEvent)
        {
            if (currentEvent.State == TouchState.LeftClick)
            {
                _isTouch = true;
            }

            if (currentEvent.State == TouchState.End)
            {
                _isTouch = false;
                DraggingObjectLogic.IsTouchStart = false;
            }
        }

        public void OnEventTrigger(PlacementEvent currentEvent)
        {
            RightClickControl.Instance.IsClickable = true;
            bool isPlaced = currentEvent.IsPlaced;
            if (isPlaced && !ReferenceEquals(_spawnedDragableObject, null))
                PlacementController.Place((IPlaceable) _spawnedDragableObject, _spawnedDragableObject.PlaceableCellList);
            _spawnedDragableObject?.Object.SetActive(isPlaced);
            _spawnedDragableObject = null;
        }
    }
}