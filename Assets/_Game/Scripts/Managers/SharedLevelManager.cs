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
        
        public SoldierController SpawnSoldier(string name, Vector3 position)
        {
            SoldierController newSoldier = (SoldierController)_soldierPool.GetPooledObject();
            newSoldier.transform.position = position;
            newSoldier.SetType(name);
            newSoldier.gameObject.SetActive(true);
            return newSoldier;
        }
    }
}