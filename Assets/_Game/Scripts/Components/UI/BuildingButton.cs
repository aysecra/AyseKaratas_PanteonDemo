using PanteonDemo.Event;
using PanteonDemo.Interfaces;
using PanteonDemo.SO;

namespace PanteonDemo.Component
{
    public class BuildingButton : UnitButton
    {
        private BuildingUnitSO _currUnitSo;

        protected override UnitSO CurrUnitSo => _currUnitSo;

        public override void SetElementValue(UnitSO currSo, IPlaceable placeable = null)
        {
            _currUnitSo = (BuildingUnitSO) currSo;
            imgElement.sprite = currSo.Image;
            textTitle.text = currSo.Name;
            textHealth.text = currSo.Health.ToString();
        }

        public override void OnButtonClicked()
        {
            EventManager.TriggerEvent(new SpawnEvent(_currUnitSo));
        }
    }
}