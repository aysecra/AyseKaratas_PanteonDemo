using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class SharedLevelManager : PersistentSingleton<SharedLevelManager>
    {
        [SerializeField] private List<Soldier> _spawnedSoldierElements = new List<Soldier>();

        public List<Soldier> SpawnedSoldierElements => _spawnedSoldierElements;
    }
}
