using PanteonDemo.Interfaces;
using UnityEngine;

namespace PanteonDemo.Component
{
    public class SoldierPlacement : MonoBehaviour
                                    , IPlaceable
    {
        private Vector2 _size;
        private bool _isActivate;

        public Vector2 Size => _size;
        public bool IsActivate => _isActivate;

        public void SetSoldier(Vector2 size)
        {
            _size = size;
            _isActivate = false;
        }

        public void Place()
        {
            
        }
    }
}
