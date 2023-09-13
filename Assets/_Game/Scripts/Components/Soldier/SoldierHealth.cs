using PanteonDemo.Controller;
using PanteonDemo.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo.Component
{
    public class SoldierHealth : MonoBehaviour
                                , IDamageable
                                , IHitable
    {
        [SerializeField] private Image _healthbar;

        public uint Health => _health;
        public uint Damage => _damage;
        
        private uint _damage;
        private uint _health;
        private uint _fullHealth;

        public void SetSoldier(uint health, uint damage)
        {
            _fullHealth = health;
            _damage = damage;
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

        public void Hit(IDamageable target)
        {
            DamageController.TakeDamage(target, _damage);
        }
    }
}