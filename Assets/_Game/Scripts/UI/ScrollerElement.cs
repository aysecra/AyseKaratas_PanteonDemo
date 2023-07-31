using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class ScrollerElement : PoolableObject
    {
        [Header("Core Elements")] 
        [SerializeField] private Image _imgElement;
        [SerializeField] private TMPro.TextMeshProUGUI _textTitle;
        [SerializeField] private TMPro.TextMeshProUGUI _textHealth;
        [SerializeField] private TMPro.TextMeshProUGUI _textDamage;

        public void SetElementValue(Soldier soldierData)
        {
            _imgElement.sprite = soldierData.Image;
            _textTitle.text = soldierData.Title;
            _textHealth.text = soldierData.Health.ToString();
            _textDamage.text = soldierData.Damage.ToString();
        }
        
        public void OnButtonClicked()
        {
            // todo : spawn soldier
        }
    }
}
