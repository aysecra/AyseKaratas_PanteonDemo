using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public List<GridsCell> CellList { get; private set; }

        public MultipleCellPlacement(int columnCount, int rowCount)
        {
            _columnCount = columnCount;
            _rowCount = rowCount;

            int gridColumn = (int) GridSystem.Instance.ColumnCount;
            int gridRow = (int) GridSystem.Instance.RowCount;

            DownLeft = new ObjectCellInfo(gridColumn, gridRow);
            DownRight = new ObjectCellInfo(gridColumn, gridRow);
            TopLeft = new ObjectCellInfo(gridColumn, gridRow);
            TopRight = new ObjectCellInfo(gridColumn, gridRow);
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
            // Control target is in index
            if (!DownLeft.IsPlacableWithChangingAmount(rowChangeAmount, columnChangeAmount, out int targetColumnDL,
                    out int targetRowDL))
                return false;
            if (!DownRight.IsPlacableWithChangingAmount(rowChangeAmount, columnChangeAmount)) return false;
            if (!TopLeft.IsPlacableWithChangingAmount(rowChangeAmount, columnChangeAmount)) return false;
            if (!TopRight.IsPlacableWithChangingAmount(rowChangeAmount, columnChangeAmount)) return false;

            // Control area is can place / clear

            if (GridSystem.Instance.IsClearArea(targetRowDL, targetRowDL + _rowCount - 1, targetColumnDL,
                    targetColumnDL + _columnCount - 1, out List<GridsCell> cellList)) CellList = cellList;

            else return false;

            PlaceFromDownLeftCell(targetRowDL, targetColumnDL);
            return true;
        }

        public bool PlacementFromDownLeftCell(int row, int column)
        {
            if (GridSystem.Instance.IsClearArea(row, row + _rowCount - 1, column,
                    column + _columnCount - 1, out List<GridsCell> cellList)) CellList = cellList;

            else return false;

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
        private bool _isBuildingPlaced = false;

        public List<GridsCell> PlacedCellList;
        public Building CurrentBuilding => _currentBuilding;

        public BuildingType CurrentBuildingType => _currentBuildingType;
        private int _currentHealth;

        private void Start()
        {
            SetType(_currentBuildingName);
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
            _multipleCellPlacement = new MultipleCellPlacement((int) _columnCount, (int) _rowCount);
        }

        public void SetFirstPositionWithDownLeftCell(int row, int column)
        {
            _multipleCellPlacement.PlacementFromDownLeftCell(row, column);
        }

        public void SetPositionWithDownLeftCell(int rowChangeAmount, int columnChangeAmount, Vector3 cellPos)
        {
            float cellSize = GridSystem.Instance.CellSize.x;
            Vector3 changingDist =
                new Vector3(cellSize * (int) _columnCount * .5f, cellSize * (int) _rowCount * .5f, 0);
            Vector3 downLeftCorner = cellPos - new Vector3(cellSize, cellSize, 0) * .5f;
            Vector3 position = downLeftCorner + changingDist;
            transform.position = position;

            if (Raycast2DManager.SendRaycast(_currentBuildingType.RaycastPoint.position + Vector3.back * .5f,
                    Vector3.forward, out GridsCell cell))
            {
                if (_multipleCellPlacement.PlacementFromDownLeftCell((int) cell.Row, (int) cell.Column))
                {
                    _currentBuildingType.ConfirmPlacementArea.SetActive(true);
                    _currentBuildingType.DeclinePlacementArea.SetActive(false);
                }
                else
                {
                    _currentBuildingType.ConfirmPlacementArea.SetActive(false);
                    _currentBuildingType.DeclinePlacementArea.SetActive(true);
                }
            }
        }

        public bool PlaceObject()
        {
            if (Raycast2DManager.SendRaycast(_currentBuildingType.RaycastPoint.position + Vector3.back * .5f,
                    Vector3.forward, out GridsCell cell))
            {
                _multipleCellPlacement.PlacementFromDownLeftCell((int) cell.Row, (int) cell.Column);
            }

            if (_currentBuildingType.ConfirmPlacementArea.activeSelf)
            {
                PlacedCellList = _multipleCellPlacement.CellList;

                GridSystem.Instance.PlaceMultipleCellArea(_multipleCellPlacement.DownLeft.Row,
                    _multipleCellPlacement.DownLeft.Row + (int) _rowCount - 1,
                    _multipleCellPlacement.DownLeft.Column,
                    _multipleCellPlacement.DownLeft.Column + (int) _columnCount - 1);

                _currentBuildingType.ConfirmPlacementArea.SetActive(false);
                return true;
            }

            return false;
        }

        public void CloseObject()
        {
            gameObject.SetActive(false);
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
                    _currentHealth = (int) _currentBuilding.Health;
                    _currentBuildingType.Healthbar.fillAmount = (float) _currentHealth / _currentBuilding.Health;
                    break;
                }
            }
        }

        // take damage from other soldier
        public void TakeDamage(int damage)
        {
            _currentHealth = _currentHealth - damage > 0 ? _currentHealth - damage : 0;
            _currentBuildingType.Healthbar.fillAmount = (float) _currentHealth / _currentBuilding.Health;
            if (_currentHealth == 0)
                gameObject.SetActive(false);
        }
    }
}