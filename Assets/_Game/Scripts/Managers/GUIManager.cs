using PanteonDemo.Component;
using PanteonDemo.Event;
using PanteonDemo.Interfaces;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo
{
    public class GUIManager : Singleton<GUIManager>
        , EventListener<SpawnEvent>
        , EventListener<PlacementEvent>
    {
        [SerializeField] private InformationArea _informationArea;
        [SerializeField] private ProductionMenu _productionMenu;
        [SerializeField] private GameObject _placementArea;

        private void Start()
        {
            _informationArea.ClearInfoArea();
            _placementArea.SetActive(false);
        }

        public void SetInformationArea(UnitSO unitSo, IPlaceable placeable = null)
        {
            _informationArea.OpenInfo(unitSo, placeable);
        }

        public void OnClickPlacementDeclineButton()
        {
            EventManager.TriggerEvent(new PlacementEvent(false));
        }

        public void OnClickPlacementConfirmButton()
        {
            EventManager.TriggerEvent(new PlacementEvent(true));
        }

        public void ObjectNotSpawnable()
        {
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<SpawnEvent>(this);
            EventManager.EventStartListening<PlacementEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<SpawnEvent>(this);
            EventManager.EventStopListening<PlacementEvent>(this);
        }

        public void OnEventTrigger(SpawnEvent currentEvent)
        {
            if (currentEvent.Unit.GetType() == typeof(BuildingUnitSO))
                _placementArea.SetActive(true);
            _productionMenu.AllButtonActivation(false);
        }

        public void OnEventTrigger(PlacementEvent currentEvent)
        {
            _placementArea.SetActive(false);
            _productionMenu.AllButtonActivation(true);
        }
    }
}