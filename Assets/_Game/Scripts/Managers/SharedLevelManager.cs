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
        private List<Soldier> _soldierUnits = new List<Soldier>();

        [SerializeField] private List<Building> _buildingElements = new List<Building>();

        private List<SoldierController> _spawnedSoldiers = new List<SoldierController>();
        private List<BuildingController> _spawnedBuildings = new List<BuildingController>();

        public List<Soldier> SoldierUnits => _soldierUnits;
        public List<Building> BuildingElements => _buildingElements;

        public Building GetBuilding(string name)
        {
            foreach (Building building in SharedLevelManager.Instance.BuildingElements)
            {
                if (building.Name == name)
                {
                    return building;
                }
            }

            return null;
        }

        public Soldier GetSoldier(string name)
        {
            foreach (Soldier soldier in SoldierUnits)
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