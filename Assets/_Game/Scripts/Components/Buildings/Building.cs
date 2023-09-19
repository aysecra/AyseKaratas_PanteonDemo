using System.Collections.Generic;
using PanteonDemo.Interfaces;
using PanteonDemo.Logic;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Component
{
    public class Building : PoolableObject, IChangeable, IClickable
    {
        [SerializeField] private BuildingUnitSO _unitSO;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BuildingHealth buildingHealth;
        [SerializeField] private BuildingPlacement buildingPlacement;

        public UnitSO UnitySo => _unitSO;

        public void SetType(UnitSO currUnit)
        {
            _unitSO = currUnit as BuildingUnitSO;

            if (_unitSO != null)
            {
                spriteRenderer.sprite = _unitSO.Image;
                buildingHealth.SetBuilding(_unitSO.Health);
                buildingPlacement.SetBuilding(_unitSO.Size);
            }
        }

        public void OnClick()
        {
            GUIManager.Instance.SetInformationArea(_unitSO, buildingPlacement.GetComponent<IPlaceable>());
        }
    }
}