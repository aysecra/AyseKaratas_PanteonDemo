using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PanteonDemo
{
    public class SoldierController : PoolableObject
    {
        [Header("Core Elements")]
        [SerializeField] private List<SoldierType> _soldierTypes = new List<SoldierType>();
        
        [Header("Selectable Elements")]
        [SerializeField,  StringInList(typeof(PropertyDrawersHelper), "AllSoldierNames")]
        private string _currentSoldierName;

        private Soldier _currentSoldier;

        public Soldier CurrentSoldier => _currentSoldier;
        

        public void Move(Vector3[] path, float durationPerCell)
        {
            transform.DOPath(path, durationPerCell * path.Length);
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
    }
}