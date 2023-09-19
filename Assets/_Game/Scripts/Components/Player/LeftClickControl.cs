using PanteonDemo.Controller;
using PanteonDemo.Enum;
using PanteonDemo.Event;
using PanteonDemo.Interfaces;
using PanteonDemo.Logic;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Component
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
            if(IsClickable)
            {
                if (_spawnedDragableObject != null && _isTouch)
                    DraggingObjectLogic.DetectAndDragObject(_mainCamera, _spawnedDragableObject);
                else if (_spawnedDragableObject == null && _isTouch)
                    DetectingAndOpenInfoArea.OpenInfo(_mainCamera);
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
            if (isPlaced)
                PlacementController.Place((IPlaceable) _spawnedDragableObject,
                    _spawnedDragableObject.PlaceableCellList);
            _spawnedDragableObject.Object.SetActive(isPlaced);
            _spawnedDragableObject = null;
        }
    }
}