using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class PlayerController : MonoBehaviour
        , EventListener<InputEvent>
    {
        private bool _isStartLeftClick = false;
        private bool _isStartRightClick = false;

        private Vector3 _firstTouchPosition;
        
        private void OnLeftClickSoldier(SoldierController soldierContoller)
        {
            GUIManager.Instance.SetInformationArea(soldierContoller.CurrentSoldier);
        }
        
        private void OnLeftClickBuilding(BuildingController buildingController)
        {
            GUIManager.Instance.SetInformationArea(buildingController.CurrentBuilding);
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<InputEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<InputEvent>(this);
        }

        public void OnEventTrigger(InputEvent currentEvent)
        {
            if (currentEvent.State == TouchState.LeftClick)
            {
                _isStartLeftClick = true;
                _firstTouchPosition = currentEvent.Position;
                
                if (Raycast2DManager.DetectTouchedObject(_firstTouchPosition, out SoldierController soldierController))
                {
                    OnLeftClickSoldier(soldierController);
                }
                
                if (Raycast2DManager.DetectTouchedObject(_firstTouchPosition, out BuildingController buildingController))
                {
                    OnLeftClickBuilding(buildingController);
                }
            }

            else if (currentEvent.State == TouchState.RightClick)
            {
                _isStartRightClick = true;
            }
            else if (currentEvent.State == TouchState.End)
            {
                _isStartLeftClick = false;
                _isStartRightClick = false;
            }
        }
    }
}