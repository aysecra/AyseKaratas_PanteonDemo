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
        public string _currentSoldierName;
        public void Move(Vector3[] path, float durationPerCell)
        {
            transform.DOPath(path, durationPerCell * path.Length);
        }

        private void Start()
        {
            SetSoldierType(_currentSoldierName);
        }

        private void OnValidate()
        {
            SetSoldierType(_currentSoldierName);
        }

        public void SetSoldierType(string soldierName)
        {
            foreach (SoldierType soldierType in _soldierTypes)
            {
                soldierType.SoldierObject.SetActive(soldierName == soldierType.Name);
            }
        }
    }
}