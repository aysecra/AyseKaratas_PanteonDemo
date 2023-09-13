using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class SharedLevelManager : PersistentSingleton<SharedLevelManager>
    {
        [Header("Object Pools")] [SerializeField]
        private ObjectPool _soldierPool;

        [SerializeField] private ObjectPool _buildingPool;

        [Header("Unit Lists")] [SerializeField]
        private List<SoldierData> _soldierUnits = new List<SoldierData>();

        [SerializeField] private List<BuildingData> _buildingElements = new List<BuildingData>();

        private List<SoldierController> _spawnedSoldiers = new List<SoldierController>();
        private List<BuildingController> _spawnedBuildings = new List<BuildingController>();

        public List<SoldierData> SoldierUnits => _soldierUnits;
        public List<BuildingData> BuildingElements => _buildingElements;

        public BuildingData GetBuilding(string name)
        {
            foreach (BuildingData building in SharedLevelManager.Instance.BuildingElements)
            {
                if (building.Name == name)
                {
                    return building;
                }
            }

            return null;
        }

        public SoldierData GetSoldier(string name)
        {
            foreach (SoldierData soldier in SoldierUnits)
            {
                if (soldier.Name == name)
                {
                    return soldier;
                }
            }

            return null;
        }

        public T SpawnElement<T>(string name, Vector3 position) where T : PoolableObject
        {
            T newElement = null;
            if (typeof(T) == typeof(SoldierController))
            {
                newElement = (T) _soldierPool.GetPooledObject();
                (newElement as SoldierController).SetType(name);
            }
            else if (typeof(T) == typeof(BuildingController))
            {
                newElement = (T) _buildingPool.GetPooledObject();
                (newElement as BuildingController).SetType(name);
            }

            if (newElement != null)
            {
                newElement.transform.position = position;
                newElement.gameObject.SetActive(true);
            }

            return newElement;
        }
    }
}