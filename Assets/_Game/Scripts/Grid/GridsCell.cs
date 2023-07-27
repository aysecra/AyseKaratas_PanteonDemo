using System;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class GridsCell : MonoBehaviour,
        EventListener<GridCellsIndexCalcEvent>
    {
        [Header("Cell Info")] [SerializeField] private uint _row;
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