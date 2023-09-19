using System;
using PanteonDemo.Interfaces;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Manager
{
    public class UpdateManager : MonoBehaviour
    {
        [SerializeField] private UpdateManagementSO updateSO;

        private void Start()
        {
            UpdateCoroutine();
        }

        public void UpdateCoroutine()
        {
            StartCoroutine(updateSO.IUpdate());
        }
        
    }
}
