using System;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class ObjectCellInfo
    {
        private readonly int _columnCount;
        private readonly int _rowCount;

        public ObjectCellInfo(int columnCount, int rowCount)
        {
            _columnCount = columnCount;
            _rowCount = rowCount;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }

        public void SetRowAndColumn(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool IsPlacableWithChangingAmount(int rowChangeAmount, int columnChangeAmount, out int targetColumn,
            out int targetRow)
        {
            targetRow = Row + rowChangeAmount;
            targetColumn = Column + columnChangeAmount;

            bool result = targetRow >= 0 && targetRow < _rowCount && targetColumn >= 0 && targetColumn < _columnCount;

            return result;
        }

        public bool IsPlacableWithChangingAmount(int rowChangeAmount, int columnChangeAmount)
        {
            int targetRow = Row + rowChangeAmount;
            int targetColumn = Column + columnChangeAmount;

            bool result = targetRow >= 0 && targetRow < _rowCount && targetColumn >= 0 && targetColumn < _columnCount;

            return result;
        }
    }

    public class MultipleCellPlacement
    {
        private readonly int _columnCount;
        private readonly int _rowCount;
        public ObjectCellInfo TopRight { get; private set; }
        public ObjectCellInfo TopLeft { get; private set; }
        public ObjectCellInfo DownRight { get; private set; }
        public ObjectCellInfo DownLeft { get; private set; }

        public MultipleCellPlacement(int columnCount, int rowCount)
        {
            _columnCount = columnCount;
            _rowCount = rowCount;

            DownLeft = new ObjectCellInfo(columnCount, rowCount);
            DownRight = new ObjectCellInfo(columnCount, rowCount);
            TopLeft = new ObjectCellInfo(columnCount, rowCount);
            TopRight = new ObjectCellInfo(columnCount, rowCount);
        }

        // place building from use down left cell info
        private void PlaceFromDownLeftCell(int rowDL, int columnDL)
        {
            DownLeft.SetRowAndColumn(rowDL, columnDL);
            DownRight.SetRowAndColumn(rowDL, columnDL + _columnCount - 1);
            TopLeft.SetRowAndColumn(rowDL + _rowCount - 1, columnDL);
            TopRight.SetRowAndColumn(rowDL + _rowCount - 1, columnDL + _columnCount - 1);
        }

        // control is can placed and place when its true
        public bool IsPlacable(int rowChangeAmount, int columnChangeAmount)
        {
            if (!DownLeft.IsPlacableWithChangingAmount(rowChangeAmount, columnChangeAmount, out int targetColumnDL,
                    out int targetRowDL))
                return false;
            if (!DownRight.IsPlacableWithChangingAmount(rowChangeAmount, columnChangeAmount)) return false;
            if (!TopLeft.IsPlacableWithChangingAmount(rowChangeAmount, columnChangeAmount)) return false;
            if (!TopRight.IsPlacableWithChangingAmount(rowChangeAmount, columnChangeAmount)) return false;

            if (!GridSystem.Instance.IsClearArea(targetRowDL, targetRowDL + _rowCount - 1, targetColumnDL,
                    targetColumnDL + _columnCount - 1)) return false;

            PlaceFromDownLeftCell(targetRowDL, targetColumnDL);
            return true;
        }

        public bool FirstPlacementFromDownLeftCell(int row, int column)
        {
            if (!GridSystem.Instance.IsClearArea(row, row + _rowCount - 1, column,
                    column + _columnCount - 1)) return false;

            PlaceFromDownLeftCell(row, column);
            return true;
        }
    }

    public class BuildingController : PoolableObject
    {
        [Header("Core Elements")] [SerializeField]
        private BoxCollider2D _collider;

        [SerializeField] private List<BuildingType> _buildingTypes = new List<BuildingType>();

        [Header("Selectable Elements")]
        [SerializeField, StringInList(typeof(PropertyDrawersHelper), "AllBuildingNames")]
        private string _currentBuildingName;

        private uint _columnCount;
        private uint _rowCount;
        private Building _currentBuilding;
        private BuildingType _currentBuildingType;
        private GridsCell _currentCell;
        private MultipleCellPlacement _multipleCellPlacement;

        public List<GridsCell> PlacedCellList { get; set; }
        public Building CurrentBuilding => _currentBuilding;

        private void Start()
        {
            SetType(_currentBuildingName);
            _multipleCellPlacement = new MultipleCellPlacement((int) _rowCount, (int) _columnCount);
        }

        private void Update()
        {
            // DetectCellChanging();
        }

        private void OnValidate()
        {
            SetBuildingType(_currentBuildingName);
        }

        // set build type by name that can be changed from other classes
        public void SetType(string buildingName)
        {
            _currentBuildingName = buildingName;
            SetBuildingType(buildingName);
            SetCollider(buildingName);
            SetCurrentBuilding();
        }

        public void SetPosition()
        {
        }

        // open to object by name
        private void SetBuildingType(string buildingName)
        {
            foreach (BuildingType buildingType in _buildingTypes)
            {
                bool isTarget = buildingName == buildingType.Name;
                buildingType.BuildingObject.SetActive(isTarget);
                if (isTarget) _currentBuildingType = buildingType;
            }
        }

        // set collider size according to building type and its size
        private void SetCollider(string buildingName)
        {
            Building building = SharedLevelManager.Instance.GetBuilding(buildingName);

            if (building != null)
            {
                float size = GridSystem.Instance.CellSize.x;
                _rowCount = building.Row;
                _columnCount = building.Column;

                _collider.size = new Vector2(_columnCount * size, _rowCount * size);
            }
        }

        // set current building class info from shared level manager
        private void SetCurrentBuilding()
        {
            foreach (var building in SharedLevelManager.Instance.BuildingElements)
            {
                if (building.Name == _currentBuildingName)
                {
                    _currentBuilding = building;
                    break;
                }
            }
        }

        private void DetectCellChanging()
        {
            if (Raycast2DManager.SendRaycast(_currentBuildingType.RaycastPoint.position + Vector3.back * .5f,
                    Vector3.forward,
                    out GridsCell cell))
            {
                if (_currentCell == null)
                {
                    _currentCell = cell;
                    _multipleCellPlacement.FirstPlacementFromDownLeftCell((int) cell.Row, (int) cell.Column);
                }
                else if (_currentCell.Column != cell.Column && _currentCell.Row != cell.Row)
                {
                    int changingColumnAmount = (int) cell.Column - (int) _currentCell.Column;
                    int changingRowAmount = (int) cell.Row - (int) _currentCell.Row;

                    bool isPlacable = _multipleCellPlacement.IsPlacable(changingRowAmount, changingColumnAmount);

                    if (isPlacable)
                    {
                    }
                }
            }
        }
    }
}