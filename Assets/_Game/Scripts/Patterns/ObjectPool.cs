using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace StrategyDemo
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] protected Transform parentObject;
        [SerializeField] private PoolableObject poolableObject;
        [SerializeField] private uint amountToPool;
        [SerializeField] private bool isAllObjectActive;
        
        private List<PoolableObject> _pooledObjects;

        protected uint AmountToPool
        {
            get => amountToPool;
            set => amountToPool = value;
        }

        public PoolableObject PoolableObject => poolableObject;


        protected virtual void Start()
        {
            _pooledObjects = new List<PoolableObject>();
            GameObject newObject;
            
            for(int i = 0; i < amountToPool; i++)
            {
                AddNewObject();
            }
        }
        
        private void AddNewObject()
        {
            GameObject newObject = Instantiate(poolableObject.gameObject, parentObject);
            newObject.SetActive(isAllObjectActive);
            PoolableObject newPoolableObject = newObject.GetComponent<PoolableObject>();
            _pooledObjects.Add(newPoolableObject);
        }
        
        public PoolableObject GetPooledObject()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                if(!_pooledObjects[i].gameObject.activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }
            
            AddNewObject();
            return _pooledObjects[^1];
        }
    }
}