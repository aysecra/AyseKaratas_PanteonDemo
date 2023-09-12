using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class PlayerController : Singleton<PlayerController>
        , EventListener<InputEvent>
        , EventListener<BuildingSpawnEvent>
        , EventListener<BuildingPlaceEvent>
    {
        [SerializeField] private Transform _pivotPoint;

        public bool IsClickable { get; set; }
        private bool _isStartLeftClick = false;
        private bool _isStartRightClick = false;

        private Vector3 _firstLeftClickPosition;
        private Vector3 _firstRightClickPosition;
        private GameObject _clickedObject;
        private bool _isSwipeBuilding = false;
        private GridsCell _buildingCell;
        private Vector2 _prevPosition;
        private BuildingController _buildingController;
        private bool _isStartSwipe;

        private void Awake()
        {
            IsClickable = true;
        }

        private void Update()
        {
            if (_isSwipeBuilding && _isStartLeftClick)
            {
                Vector2 currentMousePoint = InputManager.Instance.CurrentPosition;
                Vector3 distance = currentMousePoint - _prevPosition;
                _prevPosition = currentMousePoint;

                var pivotScreenPos = Camera.main.WorldToScreenPoint(_pivotPoint.position);
                pivotScreenPos += distance;
                pivotScreenPos.z = 0;
                if (_isStartSwipe)
                    _pivotPoint.position = Camera.main.ScreenToWorldPoint(pivotScreenPos);

                if (Raycast2DManager.DetectTouchedObject(pivotScreenPos, out GridsCell cell))
                {
                    if (!_isStartSwipe)
                    {
                        _isStartSwipe = true;
                    }

                    if (_buildingCell != null && cell != _buildingCell)
                    {
                        _buildingController.SetPositionWithDownLeftCell((int) cell.Row - (int) _buildingCell.Row,
                            (int) cell.Column - (int) _buildingCell.Column, cell.transform.position);
                        _buildingCell = cell;
                    }
                }
            }
        }

        private void OnLeftClickSoldier(SoldierController soldierContoller)
        {
            GUIManager.Instance.SetInformationArea(soldierContoller.CurrentSoldier);
        }

        private void OnLeftClickBuilding(BuildingController buildingController)
        {
            GUIManager.Instance.SetInformationArea(buildingController.CurrentBuilding, buildingController);
        }

        private void OnRightClickCell(GridsCell cell, SoldierController soldierController)
        {
            List<GridsCellBase> pathCellList =
                Pathfinding.FindPath(soldierController.PlacedCell.CellBase, cell.CellBase);

            if (pathCellList != null && pathCellList.Count > 0)
            {
                Vector3[] path = new Vector3[pathCellList.Count];

                for (int i = 0; i < pathCellList.Count; i++)
                {
                    path[i] = pathCellList[i].CellObjectScript.transform.position;
                }

                soldierController.Move(path, cell);
            }
        }

        private void OnRightClickBuilding(BuildingController buildingController, SoldierController soldierController)
        {
            // control empty cell in soldier cells neighbours
            GridsCellBase target = null;

            foreach (GridsCell placedCell in buildingController.PlacedCellList)
            {
                foreach (var neighbour in placedCell.CellBase.Neighbors)
                {
                    if (neighbour.IsWalkable)
                    {
                        target = neighbour;
                        break;
                    }
                }

                if (target != null)
                    break;
            }

            if (target != null)
            {
                List<GridsCellBase> pathCellList =
                    Pathfinding.FindPath(soldierController.PlacedCell.CellBase, target);

                if (pathCellList != null && pathCellList.Count > 0)
                {
                    Vector3[] path = new Vector3[pathCellList.Count];

                    for (int i = 0; i < pathCellList.Count; i++)
                    {
                        path[i] = pathCellList[i].CellObjectScript.transform.position;
                    }

                    soldierController.Move(path, pathCellList[^1].CellObjectScript, buildingController);
                }
            }
        }

        private void OnRightClickSoldier(SoldierController soldier, SoldierController soldierController)
        {
            GridsCellBase target = null;

            foreach (var neighbour in soldier.PlacedCell.CellBase.Neighbors)
            {
                if (neighbour.IsWalkable)
                {
                    target = neighbour;
                    break;
                }
            }

            if (target != null)
            {
                List<GridsCellBase> pathCellList =
                    Pathfinding.FindPath(soldierController.PlacedCell.CellBase, target);

                if (pathCellList != null && pathCellList.Count > 0)
                {
                    Vector3[] path = new Vector3[pathCellList.Count];

                    for (int i = 0; i < pathCellList.Count; i++)
                    {
                        path[i] = pathCellList[i].CellObjectScript.transform.position;
                    }

                    soldierController.Move(path, pathCellList[^1].CellObjectScript, soldier);
                }
            }
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<InputEvent>(this);
            EventManager.EventStartListening<BuildingSpawnEvent>(this);
            EventManager.EventStartListening<BuildingPlaceEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<InputEvent>(this);
            EventManager.EventStopListening<BuildingSpawnEvent>(this);
            EventManager.EventStopListening<BuildingPlaceEvent>(this);
        }

        public void OnEventTrigger(InputEvent currentEvent)
        {
            if (IsClickable && !_isSwipeBuilding)
            {
                if (currentEvent.State == TouchState.LeftClick)
                {
                    _isStartLeftClick = true;
                    _firstLeftClickPosition = currentEvent.Position;

                    if (Raycast2DManager.DetectTouchedObject(_firstLeftClickPosition,
                            out SoldierController soldierController))
                    {
                        soldierController.OpenClickedArea();
                        _clickedObject = soldierController.gameObject;
                        OnLeftClickSoldier(soldierController);
                    }

                    else if (Raycast2DManager.DetectTouchedObject(_firstLeftClickPosition,
                                 out BuildingController buildingController))
                    {
                        if (_clickedObject != null && _clickedObject.TryGetComponent(out SoldierController soldier))
                        {
                            soldier.CloseClickedArea();
                        }
                        _clickedObject = buildingController.gameObject;
                        OnLeftClickBuilding(buildingController);
                    }
                }

                else if (currentEvent.State == TouchState.RightClick)
                {
                    _isStartRightClick = true;
                    _firstRightClickPosition = currentEvent.Position;

                    if (Raycast2DManager.DetectTouchedObject(_firstRightClickPosition,
                            out BuildingController buildingController))
                    {
                        if (_clickedObject != null && _clickedObject.TryGetComponent(out SoldierController soldier))
                        {
                            OnRightClickBuilding(buildingController, soldier);
                            _clickedObject = null;
                        }
                    }

                    else if (Raycast2DManager.DetectTouchedObject(_firstRightClickPosition,
                                 out SoldierController soldier1))
                    {
                        if (_clickedObject != null && _clickedObject.TryGetComponent(out SoldierController soldier))
                        {
                            if (soldier != soldier1)
                                OnRightClickSoldier(soldier1, soldier);
                            _clickedObject = null;
                        }
                    }

                    else if (Raycast2DManager.DetectTouchedObject(_firstRightClickPosition, out GridsCell cell))
                    {
                        if (_clickedObject != null && _clickedObject.TryGetComponent(out SoldierController soldier))
                        {
                            OnRightClickCell(cell, soldier);
                            _clickedObject = null;
                        }
                    }
                }
            }
            else if (_isSwipeBuilding)
            {
                if (currentEvent.State == TouchState.LeftClick)
                {
                    if (Raycast2DManager.DetectTouchedObject(currentEvent.Position,
                            out BuildingController buildingController))
                    {
                        if (buildingController == _buildingController)
                        {
                            _pivotPoint.position = buildingController.CurrentBuildingType.RaycastPoint.position;

                            if (Raycast2DManager.SendRaycast(_pivotPoint.position + Vector3.back * .5f,
                                    Vector3.forward, out GridsCell cell))
                            {
                                _buildingCell = cell;
                                _prevPosition = _firstLeftClickPosition;
                                _firstLeftClickPosition = currentEvent.Position;
                                _isStartLeftClick = true;
                            }
                        }
                    }
                }
            }

            if (currentEvent.State == TouchState.End)
            {
                _isStartSwipe = false;
                _isStartLeftClick = false;
                _isStartRightClick = false;
            }
        }

        public void OnEventTrigger(BuildingSpawnEvent currentEvent)
        {
            _isSwipeBuilding = true;
            IsClickable = false;
            _buildingController = currentEvent.SpawnedBuilding;
        }

        public void OnEventTrigger(BuildingPlaceEvent currentEvent)
        {
            _isSwipeBuilding = false;
            IsClickable = true;
            _buildingController = null;
        }
    }
}