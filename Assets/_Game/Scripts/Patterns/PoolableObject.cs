using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public class PoolableObject : MonoBehaviour
    {
        public void ExecuteObject()
        {
            gameObject.SetActive(true);
        }

        public void DestroyObject()
        {
            gameObject.SetActive(false);
        }
    }
}
