using System;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    [Serializable]
    public class GridsCellBase
    {
        public GridsCell CellObjectScript;

        public GridsCellBase Connection;

        // distance from the cell to the start cell 
        public float G;

        // distance from the cell to the target cell 
        public float H;
        public float F => G + H;
        public bool Walkable = true;

        public List<GridsCellBase> Neighbors;

        public float GetDistance(GridsCellBase neighbor)
        {
            return Vector3.Distance(CellObjectScript.transform.position, neighbor.CellObjectScript.transform.position);
        }
    }

    public class GridsCell : MonoBehaviour
        , EventListener<GridCellsIndexCalcEvent>
    {
        [Header("Cell Info")] [SerializeField] private uint _row;
        [SerializeField] private uint _column;

        [SerializeField] private GridsCellBase _gridsCellBase;
        private GridSystem _gridSystem;

        public uint Column
        {
            get => _column;
            set => _column = value;
        }

        public uint Row
        {
            get => _row;
            set => _row = value;
        }

        public GridsCellBase CellBase => _gridsCellBase;

        private void Start()
        {
            _gridSystem = GridSystem.Instance;
        }

        public void GetHexagonNeighbours()
        {
            List<GridsCellBase> neighbours = new List<GridsCellBase>();

            // get down neighbours
            if (_row > 0)
            {
                neighbours.Add(_gridSystem.GetCell((int) (_row - 1), (int) _column).CellBase);
            }

            // get up neighbours
            if (_row < _gridSystem.RowCount - 1)
            {
                GridsCell cell = _gridSystem.GetCell((int) (_row + 1), (int) _column);
                neighbours.Add(cell.CellBase);
            }

            // get left neighbours
            if (_column > 0)
            {
                neighbours.Add(_gridSystem.GetCell((int) _row, (int) (_column - 1)).CellBase);
            }

            // get right neighbours
            if (_column < _gridSystem.ColumnCount - 1)
            {
                neighbours.Add(_gridSystem.GetCell((int) _row, (int) (_column + 1)).CellBase);
            }

            _gridsCellBase.Neighbors = neighbours;
        }

        public void SetCellBase()
        {
            _gridsCellBase = new GridsCellBase() {CellObjectScript = this};
        }

        private void OnEnable()
        {
            this.EventStartListening<GridCellsIndexCalcEvent>();
        }

        private void OnDisable()
        {
            this.EventStopListening<GridCellsIndexCalcEvent>();
        }

        public void OnEventTrigger(GridCellsIndexCalcEvent currentEvent)
        {
            if (_gridSystem == null)
                _gridSystem = GridSystem.Instance;

            GetHexagonNeighbours();
        }
    }
}