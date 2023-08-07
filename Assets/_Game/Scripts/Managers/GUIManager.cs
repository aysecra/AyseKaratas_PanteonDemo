using UnityEngine;

namespace PanteonDemo
{
    /// <summary>
    /// Event is triggered when building is spawned and placement is completed 
    /// </summary>
    public struct BuildingPlaceEvent
    {
        public BuildingController SpawnedBuilding { get; private set; }

        public BuildingPlaceEvent(BuildingController buildingController)
        {
            SpawnedBuilding = buildingController;
        }
    }

    public class GUIManager : Singleton<GUIManager>,
        EventListener<BuildingSpawnEvent>
    {
        [SerializeField] private InformationArea _informationArea;
        [SerializeField] private GameObject _placementArea;

        private BuildingController _spawnedBuildingController;

        private void Start()
        {
            _informationArea.ClearInfoArea();
            _placementArea.SetActive(false);
        }

        public void SetInformationArea(Soldier soldier)
        {
            _informationArea.OpenInfo(soldier);
        }

        public void SetInformationArea(Building building, BuildingController selectedBuilding)
        {
            _informationArea.OpenInfo(building, selectedBuilding);
        }

        public void OnClickPlacementDeclineButton()
        {
            _spawnedBuildingController.CloseObject();
            EventManager.TriggerEvent(new BuildingPlaceEvent(null));
            _placementArea.SetActive(false);
        }

        public void OnClickPlacementConfirmButton()
        {
            if (_spawnedBuildingController.PlaceObject())
            {
                EventManager.TriggerEvent(new BuildingPlaceEvent(_spawnedBuildingController));
                _placementArea.SetActive(false);
            }
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<BuildingSpawnEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<BuildingSpawnEvent>(this);
        }

        public void OnEventTrigger(BuildingSpawnEvent currentEvent)
        {
            _spawnedBuildingController = currentEvent.SpawnedBuilding;
            _placementArea.SetActive(true);
        }
    }
}