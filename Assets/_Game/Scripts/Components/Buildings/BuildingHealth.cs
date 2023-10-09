using StrategyDemo.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace StrategyDemo.Component
{
    public class BuildingHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private Image _healthbar;

        public uint Health => _health;
        
        private uint _health;
        private uint _fullHealth;

        public void SetBuilding(uint health)
        {
            _fullHealth = health;
            RestoreHealth();
        }

        public void TakeDamage(uint damage)
        {
            _health -= damage;
            _healthbar.fillAmount = (float) _health / _fullHealth;
            
            if(_health <= 0)
                Die();
        }

        public void RestoreHealth()
        {
            _health = _fullHealth;
            _healthbar.fillAmount = (float) _health / _fullHealth;
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }
    }
}