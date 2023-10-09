using System;
using StrategyDemo.Interfaces;
using StrategyDemo.SO;
using UnityEngine;

namespace StrategyDemo.Manager
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
