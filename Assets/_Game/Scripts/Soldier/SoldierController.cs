using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PanteonDemo
{
    public class SoldierController : PoolableObject
    {
        [Header("Core Elements")] [SerializeField]
        private List<SoldierType> _soldierTypes = new List<SoldierType>();

        [Header("Selectable Elements")] [SerializeField, StringInList(typeof(PropertyDrawersHelper), "AllSoldierNames")]
        private string _currentSoldierName;

        [SerializeField] private float _durationPerCell = .25f;

        private Soldier _currentSoldier;

        public GridsCell PlacedCell { get; set; }
        public Soldier CurrentSoldier => _currentSoldier;


        public void Move(Vector3[] path, GridsCell targetCell)
        {
            transform.DOPath(path, _durationPerCell * path.Length)
                .OnStart((() =>
                {
                    PlayerController.Instance.IsClickable = false;
                }))
                .OnComplete((() =>
                {
                    PlayerController.Instance.IsClickable = true;
                    PlacedCell.CellBase.IsWalkable = true;
                    targetCell.CellBase.IsWalkable = false;
                    PlacedCell = targetCell;
                }));
        }

        private void Start()
        {
            SetType(_currentSoldierName);
        }

        private void OnValidate()
        {
            SetSoldierType(_currentSoldierName);
        }

        public void SetType(string soldierName)
        {
            _currentSoldierName = soldierName;
            SetSoldierType(soldierName);
            SetCurrentSoldier();
        }

        private void SetSoldierType(string soldierName)
        {
            foreach (SoldierType soldierType in _soldierTypes)
            {
                soldierType.SoldierObject.SetActive(soldierName == soldierType.Name);
            }
        }

        private void SetCurrentSoldier()
        {
            foreach (var soldier in SharedLevelManager.Instance.SoldierUnits)
            {
                if (soldier.Name == _currentSoldierName)
                {
                    _currentSoldier = soldier;
                    break;
                }
            }
        }

        public void SpawnSoldier()
        {
        }
    }
}