using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PanteonDemo
{
    public class BuildingController : PoolableObject
    {
        [Header("Core Elements")]
        [SerializeField] private List<BuildingType> _buildingTypes = new List<BuildingType>();
        
        [Header("Selectable Elements")]
        [SerializeField,  StringInList(typeof(PropertyDrawersHelper), "AllBuildingNames")]
        public string _currentBuildingName;
        
        private uint column;
        private uint row;
        
        private void Start()
        {
            SetBuildingType(_currentBuildingName);
        }

        private void OnValidate()
        {
            SetBuildingType(_currentBuildingName);
        }

        public void SetBuildingType(string buildingName)
        {
            foreach (BuildingType buildingType in _buildingTypes)
            {
                buildingType.SoldierObject.SetActive(buildingName == buildingType.Name);
            }
        }
    }
}
