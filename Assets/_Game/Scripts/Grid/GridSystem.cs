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

    public class GridSystem : MonoBehaviour
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

        // [Button(ButtonSizes.Medium)]
        // public void Indexing()
        // {
        //     _cellList = new List<GridsCell>();
        //     GetGridCells();
        // }
        //
        // [Button(ButtonSizes.Medium)]
        // public void Clear()
        // {
        //     while (_tilemap.transform.childCount > 0)
        //     {
        //         foreach (Transform child in _tilemap.transform)
        //         {
        //             DestroyImmediate(child.gameObject);
        //         }
        //     }
        // }

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


        public GridsCell GetCell(int row, int column)
        {
            if (column >= 0 && column < _columnCount && row >= 0 && row < _rowCount)
                return _cellArray[row, column];
            return null;
        }
    }
}