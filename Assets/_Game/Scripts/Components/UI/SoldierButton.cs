using PanteonDemo.Event;
using PanteonDemo.Interfaces;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Component
{
    public class SoldierButton : UnitButton
    {
        [SerializeField] private TMPro.TextMeshProUGUI textDamage;
        
        private SoldierUnitSO _currUnitSo;
        private IPlaceable _spawnerPlaceable;

        protected override UnitSO CurrUnitSo => _currUnitSo;
        public IPlaceable SpawnerPlaceable  => _spawnerPlaceable;
        
        public override void SetElementValue(UnitSO currSo, IPlaceable placeable = null)
        {
            _spawnerPlaceable = placeable;
            _currUnitSo = (SoldierUnitSO) currSo;
            imgElement.sprite = _currUnitSo.Image;
            textTitle.text = _currUnitSo.Name;
            textHealth.text = _currUnitSo.Health.ToString();
            textDamage.text = _currUnitSo.Damage.ToString();
        }


        public override void OnButtonClicked()
        {
            EventManager.TriggerEvent(new SpawnEvent(_currUnitSo, _spawnerPlaceable));
        }
    }
}