using System.Collections;
using System.Collections.Generic;
using PanteonDemo.Interfaces;
using UnityEngine;

namespace PanteonDemo.SO
{
    [CreateAssetMenu(menuName = "SO/Update Management")]
    public class UpdateManagementSO : ScriptableObject
    {
        [SerializeField, Min(0.01f)] private float timeDelay = .05f;

        private List<IUpdateListener> _updateListenersList = new List<IUpdateListener>();

        public float TimeDelay => timeDelay;

        public void AddListener(IUpdateListener listener)
        {
            if (!_updateListenersList.Contains(listener))
            {
                _updateListenersList.Add(listener);
            }
        
        }
        
        public void RemoveListener(IUpdateListener listener)
        {
            if (_updateListenersList.Contains(listener))
            {
                _updateListenersList.Remove(listener);
            }
        }
        
        public IEnumerator IUpdate()
        {
            while (true)
            {
                for (int i = 0; i < _updateListenersList.Count; ++i)
                {
                    _updateListenersList[i].ManagedUpdate();
                }

                yield return new WaitForSeconds(timeDelay);
            }
        }
    }
}