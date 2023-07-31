using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private PoolableObject _poolableObject;
        [SerializeField] private uint _amountToPool;
        
        private List<GameObject> _pooledObjects;

        protected uint amountToPool
        {
            get => _amountToPool;
            set => _amountToPool = value;
        }


        protected virtual void Start()
        {
            _pooledObjects = new List<GameObject>();
            GameObject newObject;
            
            for(int i = 0; i < _amountToPool; i++)
            {
                newObject = Instantiate(_poolableObject.gameObject, transform);
                newObject.SetActive(false);
                _pooledObjects.Add(newObject);
            }
        }
        
        public GameObject GetPooledObject()
        {
            for(int i = 0; i < _amountToPool; i++)
            {
                if(!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }
            return null;
        }
    }
}