using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PanteonDemo
{
    public struct GridCellsIndexCalcEvent
    {
        private bool State;

        public GridCellsIndexCalcEvent(bool state = true)
        {
            State = state;
        }
    }

    public class GridSystem : Singleton<GridSystem>
    {
        [Header("Core Elements")] [SerializeField]
        private Grid _grid;

        [SerializeField] private Tilemap _tilemap;

        [Header("Readonly Elements")] [SerializeField]
        private uint _columnCount;

        [SerializeField] private uint _rowCount;

        private List<GridsCell> _cellList = new List<GridsCell>();
        private GridsCell[,] _cellArray;
        private bool _isFinishMerging;
        private float _cellDistance;

        public Vector3 CellSize => _grid.cellSize;

        public uint ColumnCount
        {
            get => _columnCount;
            set => _columnCount = value;
        }

        public uint RowCount
        {
            get => _rowCount;
            set => _rowCount = value;
        }

        public bool IsFinishMerging
        {
            get => _isFinishMerging;
            set => _isFinishMerging = value;
        }

        public float CellDistance => _cellDistance;

        private void Start()
        {
            _rowCount = 0;
            _columnCount = 0;
            GetGridCells();
        }

        void GetGridCells()
        {
            foreach (Transform child in _tilemap.transform)
            {
                if (child.TryGetComponent(out GridsCell cell))
                {
                    _cellList.Add(cell);
                    cell.SetCellBase();
                }
            }

            if (_cellList.Count > 0)
            {
                SetSquareCellIndexes();
            }

            if (_cellList.Count > 1)
            {
                _cellDistance = Vector3.Distance(_cellArray[0, 0].transform.position,
                    _cellArray[0, 1].transform.position);
            }
        }

        #region Square Cell Methods

        void SetSquareCellIndexes()
        {
            _cellList = _cellList.OrderBy(grid => grid.transform.position.x).ToList();
            int column = 0;
            _cellList[0].Column = (uint) column;
            for (int i = 1; i < _cellList.Count; i++)
            {
                if (_cellList[i - 1].transform.position.x < _cellList[i].transform.position.x)
                {
                    column++;
                    _cellList[i].Column = (uint) column;
                }
                else if (_cellList[i - 1].transform.position.x == _cellList[i].transform.position.x)
                {
                    _cellList[i].Column = (uint) column;
                }

                if (_columnCount < _cellList[i].Column + 1)
                    _columnCount = _cellList[i].Column + 1;
            }

            _cellList = _cellList.OrderBy(grid => grid.transform.position.y).ToList();
            int row = 0;
            _cellList[0].Row = (uint) row;
            for (int i = 1; i < _cellList.Count; i++)
            {
                if (_cellList[i - 1].transform.position.y < _cellList[i].transform.position.y)
                {
                    row++;
                    _cellList[i].Row = (uint) row;
                }
                else if (_cellList[i - 1].transform.position.y == _cellList[i].transform.position.y)
                {
                    _cellList[i].Row = (uint) row;
                }

                if (_rowCount < _cellList[i].Row + 1)
                    _rowCount = _cellList[i].Row + 1;
            }

            uint lastValue = _cellList[^1].Row;

            for (int i = _cellList.Count - 2; i >= 0; i--)
            {
                if (lastValue != _cellList[i].Row)
                {
                    break;
                }
            }

            PlaceSquareCellsToArray();
        }

        void PlaceSquareCellsToArray()
        {
            int _changedGridCounter = 0;
            _cellArray = new GridsCell[_rowCount, _columnCount];
            foreach (GridsCell gridPart in _cellList)
            {
                _cellArray[gridPart.Row, gridPart.Column] = gridPart;
            }

            EventManager.TriggerEvent(new GridCellsIndexCalcEvent());
        }

        #endregion

        // get specific cell according to row and column
        public GridsCell GetCell(int row, int column)
        {
            if (column >= 0 && column < _columnCount && row >= 0 && row < _rowCount)
                return _cellArray[row, column];
            return null;
        }

        // returns a placable / clear cell
        public GridsCell GetEmptyACell()
        {
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _columnCount; j++)
                {
                    if (_cellArray[i, j].CellBase.IsWalkable)
                        return _cellArray[i, j];
                }
            }

            return null;
        }

        // returns a placable / clear area
        public List<GridsCell> GetEmptyArea(uint rowCount, uint columnCount)
        {
            int rowCounter = 0;
            int columnCounter = 0;

            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _columnCount; j++)
                {
                    if (_cellArray[i, j].CellBase.IsWalkable)
                    {
                        if (IsClearArea(i, i + (int) rowCount - 1, j, j + (int) columnCount - 1,
                                out List<GridsCell> cellList))
                        {
                            return cellList;
                        }
                    }
                }
            }

            return null;
        }

        // returns is can placable / clear area
        public bool IsClearArea(int minRow, int maxRow, int minColumn, int maxColumn)
        {
            if (minRow >= 0 && minRow < _rowCount && minColumn >= 0 && minColumn < _columnCount &&
                maxRow >= 0 && maxRow < _rowCount && maxColumn >= 0 && maxColumn < _columnCount)
            {
                for (int i = minRow; i <= maxRow; i++)
                {
                    for (int j = minColumn; j <= maxColumn; j++)
                    {
                        if (!_cellArray[i, j].CellBase.IsWalkable)
                            return false;
                    }
                }

                return true;
            }

            return false;
        }

        // returns is can placable / clear area
        private bool IsClearArea(int minRow, int maxRow, int minColumn, int maxColumn, out List<GridsCell> cellList)
        {
            cellList = new List<GridsCell>();
            if (minRow >= 0 && minRow < _rowCount && minColumn >= 0 && minColumn < _columnCount &&
                maxRow >= 0 && maxRow < _rowCount && maxColumn >= 0 && maxColumn < _columnCount)
            {
                for (int i = minRow; i <= maxRow; i++)
                {
                    for (int j = minColumn; j <= maxColumn; j++)
                    {
                        if (!_cellArray[i, j].CellBase.IsWalkable)
                            return false;

                        cellList.Add(_cellArray[i, j]);
                    }
                }

                return true;
            }

            return false;
        }

        // set not walkable area
        public void PlaceMultipleCellArea(int minRow, int maxRow, int minColumn, int maxColumn)
        {
            if (minRow >= 0 && minRow < _rowCount && minColumn >= 0 && minColumn < _columnCount &&
                maxRow >= 0 && maxRow < _rowCount && maxColumn >= 0 && maxColumn < _columnCount)
            {
                for (int i = minRow; i <= maxRow; i++)
                {
                    for (int j = minColumn; j <= maxColumn; j++)
                    {
                        _cellArray[i, j].CellBase.IsWalkable = false;
                    }
                }
            }
        }
    }
}