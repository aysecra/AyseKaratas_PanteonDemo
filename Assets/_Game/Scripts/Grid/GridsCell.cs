using System;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class GridsCellBase
    {
        public GridsCell CellObjectScript;
        public GridsCellBase Connection;
        // distance from the cell to the start cell 
        public float G;
        // distance from the cell to the target cell 
        public float H;
        public float F => G + H;
        public bool Walkable;

        public List<GridsCellBase> Neighbors;

        public float GetDistance(GridsCellBase neighbor)
        {
            return 0;
        }
    }

    public class GridsCell : MonoBehaviour
                            , EventListener<GridCellsIndexCalcEvent>
    {
        [Header("Cell Info")] 
        [SerializeField] private uint _row;
        [SerializeField] private uint _column;

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


        private void Start()
        {
        }

        private void OnValidate()
        {
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
            
        }
    }
}