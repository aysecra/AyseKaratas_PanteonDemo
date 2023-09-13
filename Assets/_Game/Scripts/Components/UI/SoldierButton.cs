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

        private SoldierData _currentSoldierDataData;
        private bool _isActive = true;
        private BuildingController _selectedBuilding;

        public void SetElementValue(SoldierData soldierDataData, BuildingController selectedBuilding)
        {
            _selectedBuilding = selectedBuilding;
            _currentSoldierDataData = soldierDataData;
            _imgElement.sprite = soldierDataData.Image;
            _textTitle.text = soldierDataData.Name;
            _textHealth.text = soldierDataData.Health.ToString();
            _textDamage.text = soldierDataData.Damage.ToString();
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
                    SharedLevelManager.Instance.SpawnElement<SoldierController>(_currentSoldierDataData.Name,
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