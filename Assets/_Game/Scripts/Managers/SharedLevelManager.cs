using System.Collections.Generic;
using StrategyDemo.Interfaces;
using StrategyDemo.SO;
using UnityEngine;

namespace StrategyDemo
{
    public class SharedLevelManager : PersistentSingleton<SharedLevelManager>
    {
        [Header("Object Pools")] [SerializeField]
        private List<ObjectPool> _poolList;
        
        public PoolableObject GetObject(UnitSO unit)
        {
            foreach (ObjectPool pool in _poolList)
            {
                if(unit.GetType() == (((IChangeable) pool.PoolableObject).UnitySo).GetType())
                {
                    return pool.GetPooledObject();
                }
            }

            return null;
        }
    }
}