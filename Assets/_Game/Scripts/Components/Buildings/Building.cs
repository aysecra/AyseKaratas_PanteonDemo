using StrategyDemo.Interfaces;
using StrategyDemo.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace StrategyDemo.Component
{
    public class Building : PoolableObject, IChangeable, IClickable
    {
        [SerializeField] private BuildingUnitSO unitSo;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BuildingHealth buildingHealth;
        [SerializeField] private BuildingPlacement buildingPlacement;

        public UnitSO UnitySo => unitSo;

        public void SetType(UnitSO currUnit)
        {
            unitSo = currUnit as BuildingUnitSO;

            if (!ReferenceEquals(unitSo, null))
            {
                spriteRenderer.sprite = unitSo.Image;
                buildingHealth.SetBuilding(unitSo.Health);
                buildingPlacement.SetBuilding(unitSo.Size);
            }
        }

        public void OnClick()
        {
            GUIManager.Instance.SetInformationArea(unitSo, buildingPlacement.GetComponent<IPlaceable>());
        }
    }
}