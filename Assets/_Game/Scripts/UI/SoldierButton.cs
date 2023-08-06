using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class SoldierButton : PoolableObject
    {
        [Header("Core Elements")] [SerializeField]
        private Image _imgElement;

        [SerializeField] private TMPro.TextMeshProUGUI _textTitle;
        [SerializeField] private TMPro.TextMeshProUGUI _textHealth;
        [SerializeField] private TMPro.TextMeshProUGUI _textDamage;
        
        private Soldier _currentSoldierData;

        public void SetElementValue(Soldier soldierData)
        {
            _currentSoldierData = soldierData;
            _imgElement.sprite = soldierData.Image;
            _textTitle.text = soldierData.Name;
            _textHealth.text = soldierData.Health.ToString();
            _textDamage.text = soldierData.Damage.ToString();
        }

        public void OnButtonClicked()
        {
            GridsCell cell = GridSystem.Instance.GetEmptyACell();
            SoldierController soldier =  SharedLevelManager.Instance.SpawnElement<SoldierController>(_currentSoldierData.Name, cell.transform.position);
            cell.CellBase.IsWalkable = false;
            soldier.PlacedCell = cell;
        }
    }
}