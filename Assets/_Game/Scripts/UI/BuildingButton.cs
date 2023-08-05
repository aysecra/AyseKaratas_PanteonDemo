using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class BuildingButton : PoolableObject
    {
        [Header("Core Elements")] 
        [SerializeField] private Image _imgElement;
        [SerializeField] private TMPro.TextMeshProUGUI _textTitle;
        [SerializeField] private TMPro.TextMeshProUGUI _textHealth;

        public void SetElementValue(Building buildingData)
        {
            _imgElement.sprite = buildingData.Image;
            _textTitle.text = buildingData.Name;
            _textHealth.text = buildingData.Health.ToString();
        }
        
        public void OnButtonClicked()
        {
            // todo : spawn soldier
        }
    }
}
