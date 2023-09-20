using System.Collections;
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
        [SerializeField] private InformationArea informationArea;
        [SerializeField] private ProductionMenu productionMenu;
        [SerializeField] private GameObject placementArea;
        [SerializeField] private GameObject notPlaceableText;

        private void Start()
        {
            informationArea.ClearInfoArea();
            placementArea.SetActive(false);
        }

        public void SetInformationArea(UnitSO unitSo, IPlaceable placeable = null)
        {
            informationArea.OpenInfo(unitSo, placeable);
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
            StartCoroutine(IObjectNotSpawnable());
        }

        public void ActivateProductionButtons(bool isActivate)
        {
            productionMenu.AllButtonActivation(isActivate);
        }

        IEnumerator IObjectNotSpawnable()
        {
            notPlaceableText.gameObject.SetActive(true);
            EventManager.TriggerEvent(new PlacementEvent(false));
            yield return new WaitForSeconds(.75f);
            notPlaceableText.gameObject.SetActive(false);
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
            {
                placementArea.SetActive(true);
                productionMenu.AllButtonActivation(false);
            }
        }

        public void OnEventTrigger(PlacementEvent currentEvent)
        {
            placementArea.SetActive(false);
            productionMenu.AllButtonActivation(true);
        }
    }
}