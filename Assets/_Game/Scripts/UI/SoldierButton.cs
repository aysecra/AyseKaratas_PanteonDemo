using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class SoldierButton : PoolableObject
        , EventListener<BuildingSpawnEvent>
        , EventListener<BuildingPlaceEvent>
    {
        [Header("Core Elements")] [SerializeField]
        private Image _imgElement;

        [SerializeField] private TMPro.TextMeshProUGUI _textTitle;
        [SerializeField] private TMPro.TextMeshProUGUI _textHealth;
        [SerializeField] private TMPro.TextMeshProUGUI _textDamage;
        [SerializeField] private Button _button;

        private Soldier _currentSoldierData;
        private bool _isActive = true;
        private BuildingController _selectedBuilding;

        public void SetElementValue(Soldier soldierData, BuildingController selectedBuilding)
        {
            _selectedBuilding = selectedBuilding;
            _currentSoldierData = soldierData;
            _imgElement.sprite = soldierData.Image;
            _textTitle.text = soldierData.Name;
            _textHealth.text = soldierData.Health.ToString();
            _textDamage.text = soldierData.Damage.ToString();
        }

        public void OnButtonClicked()
        {
            if(_isActive)
            {
                uint beginingRow = _selectedBuilding.PlacedCellList[0].Row > 0
                    ? _selectedBuilding.PlacedCellList[0].Row-1
                    : _selectedBuilding.PlacedCellList[^1].Row < GridSystem.Instance.RowCount - 1
                        ? _selectedBuilding.PlacedCellList[^1].Row + 1
                        :_selectedBuilding.PlacedCellList[0].Row;

                GridsCell cell = GridSystem.Instance.GetEmptyACell((int) beginingRow, (int) _selectedBuilding.PlacedCellList[0].Column);
                SoldierController soldier =
                    SharedLevelManager.Instance.SpawnElement<SoldierController>(_currentSoldierData.Name,
                        cell.transform.position);
                cell.CellBase.IsWalkable = false;
                soldier.PlacedCell = cell;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.EventStartListening<BuildingSpawnEvent>(this);
            EventManager.EventStartListening<BuildingPlaceEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<BuildingSpawnEvent>(this);
            EventManager.EventStopListening<BuildingPlaceEvent>(this);
        }

        public void OnEventTrigger(BuildingSpawnEvent currentEvent)
        {
            _isActive = false;
        }

        public void OnEventTrigger(BuildingPlaceEvent currentEvent)
        {
            _isActive = true;
        }
    }
}