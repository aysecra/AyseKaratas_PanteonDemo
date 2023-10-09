using StrategyDemo.Interfaces;
using StrategyDemo.SO;
using UnityEngine;

namespace StrategyDemo.Component
{
    public class Soldier : PoolableObject
        , IChangeable
        , IClickable
    {
        [SerializeField] private SoldierUnitSO _unitSO;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SoldierHealth soldierHealth;
        [SerializeField] private SoldierMovement soldierMovement;

        public UnitSO UnitySo => _unitSO;

        public void SetType(UnitSO currUnit)
        {
            _unitSO = currUnit as SoldierUnitSO;

            if (!ReferenceEquals(_unitSO, null))
            {
                spriteRenderer.sprite = _unitSO.Image;
                soldierHealth.SetSoldier(_unitSO.Health, _unitSO.Damage);
                soldierMovement.SetSoldier(_unitSO.DurationPerCell, _unitSO.Size);
            }
        }

        public void OnClick()
        {
            GUIManager.Instance.SetInformationArea(_unitSO);
        }
    }
}