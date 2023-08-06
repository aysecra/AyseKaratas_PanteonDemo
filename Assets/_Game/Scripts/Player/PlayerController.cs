using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class PlayerController : Singleton<PlayerController>
        , EventListener<InputEvent>
    {
        public bool IsClickable { get; set; }
        private bool _isStartLeftClick = false;
        private bool _isStartRightClick = false;

        private Vector3 _firstLeftClickPosition;
        private Vector3 _firstRightClickPosition;
        private GameObject _clickedObject;

        private void Awake()
        {
            IsClickable = true;
        }

        private void OnLeftClickSoldier(SoldierController soldierContoller)
        {
            GUIManager.Instance.SetInformationArea(soldierContoller.CurrentSoldier);
        }

        private void OnLeftClickBuilding(BuildingController buildingController)
        {
            GUIManager.Instance.SetInformationArea(buildingController.CurrentBuilding);
        }

        private void OnRightClickCell(GridsCell cell, SoldierController soldierController)
        {
            List<GridsCellBase> pathCell = Pathfinding.FindPath(soldierController.PlacedCell.CellBase, cell.CellBase);
            Vector3[] path = new Vector3[pathCell.Count];

            for (int i = 0; i < pathCell.Count; i++)
            {
                path[i] = pathCell[i].CellObjectScript.transform.position;
            }

            soldierController.Move(path, cell);
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
            if (IsClickable)
            {
                if (currentEvent.State == TouchState.LeftClick)
                {
                    _isStartLeftClick = true;
                    _firstLeftClickPosition = currentEvent.Position;

                    if (Raycast2DManager.DetectTouchedObject(_firstLeftClickPosition,
                            out SoldierController soldierController))
                    {
                        _clickedObject = soldierController.gameObject;
                        OnLeftClickSoldier(soldierController);
                    }

                    if (Raycast2DManager.DetectTouchedObject(_firstLeftClickPosition,
                            out BuildingController buildingController))
                    {
                        _clickedObject = buildingController.gameObject;
                        OnLeftClickBuilding(buildingController);
                    }
                }

                else if (currentEvent.State == TouchState.RightClick)
                {
                    _isStartRightClick = true;
                    _firstRightClickPosition = currentEvent.Position;

                    if (Raycast2DManager.DetectTouchedObject(_firstRightClickPosition, out GridsCell cell))
                    {
                        if (_clickedObject != null && _clickedObject.TryGetComponent(out SoldierController soldier))
                        {
                            OnRightClickCell(cell, soldier);
                        }
                    }
                }
                else if (currentEvent.State == TouchState.End)
                {
                    _isStartLeftClick = false;
                    _isStartRightClick = false;
                }
            }
        }
    }
}