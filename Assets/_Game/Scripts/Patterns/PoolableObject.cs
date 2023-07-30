using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class PoolableObject : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _lifeTime = 0;

        public float LifeTime => _lifeTime;

        public void ExecuteObject()
        {
            gameObject.SetActive(true);
        }

        public void DestroyObject()
        {
            gameObject.SetActive(false);
        }

        IEnumerator IDestroyAfterFinishLifeTime()
        {
            yield return new WaitForSeconds(_lifeTime);
            DestroyObject();
        }

        private void OnEnable()
        {
            if (_lifeTime > 0)
            {
                StartCoroutine(IDestroyAfterFinishLifeTime());
            }
        }
    }
}
