using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class BuildingController : PoolableObject
    {
        [Header("Core Elements")] [SerializeField]
        private BoxCollider2D _collider;

        [SerializeField] private List<BuildingType> _buildingTypes = new List<BuildingType>();

        [Header("Selectable Elements")]
        [SerializeField, StringInList(typeof(PropertyDrawersHelper), "AllBuildingNames")]
        private string _currentBuildingName;

        private uint _column;
        private uint _row;
        private Building _currentBuilding;

        public Building CurrentBuilding => _currentBuilding;

        private void Start()
        {
            SetType(_currentBuildingName);
        }

        private void OnValidate()
        {
            SetBuildingType(_currentBuildingName);
        }

        public void SetType(string buildingName)
        {
            SetBuildingType(buildingName);
            SetCollider(buildingName);
            SetCurrentBuilding();
        }

        private void SetBuildingType(string buildingName)
        {
            foreach (BuildingType buildingType in _buildingTypes)
            {
                buildingType.SoldierObject.SetActive(buildingName == buildingType.Name);
            }
        }

        private void SetCollider(string buildingName)
        {
            Building building = SharedLevelManager.Instance.GetBuilding(buildingName);

            if (building != null)
            {
                float size = GridSystem.Instance.CellSize.x;
                _row = building.Row;
                _column = building.Column;

                _collider.size = new Vector2(_column * size, _row * size);
            }
        }

        private void SetCurrentBuilding()
        {
            foreach (var building in SharedLevelManager.Instance.BuildingElements)
            {
                if (building.Name == _currentBuildingName)
                {
                    _currentBuilding = building;
                    break;
                }
            }
        }
    }
}