using PanteonDemo.Interfaces;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Component
{
    public class Soldier : PoolableObject
                            , IChangeable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SoldierHealth soldierHealth;
        [SerializeField] private SoldierMovement soldierMovement;
        [SerializeField] private SoldierPlacement soldierPlacement;
        
        private SoldierUnitSO _unitSO;

        public UnitSO UnitySo => _unitSO;

        public void SetType(UnitSO currUnit)
        {
            _unitSO = currUnit as SoldierUnitSO;
            
            if (_unitSO != null)
            {
                spriteRenderer.sprite = _unitSO.Image;
                soldierHealth.SetSoldier(_unitSO.Health, _unitSO.Damage);
                soldierMovement.SetSoldier(_unitSO.DurationPerCell);
                soldierPlacement.SetSoldier(_unitSO.Size);
            }
        }
    }
}
