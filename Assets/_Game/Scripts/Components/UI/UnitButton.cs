using PanteonDemo.Interfaces;
using PanteonDemo.SO;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo.Component
{
    public abstract class UnitButton : PoolableObject
    {
        [Header("Core Elements")] [SerializeField]
        protected Image imgElement;

        [SerializeField] protected TMPro.TextMeshProUGUI textTitle;
        [SerializeField] protected TMPro.TextMeshProUGUI textHealth;
        [SerializeField] protected Button button;

        protected bool _isActive = true;

        protected abstract UnitSO CurrUnitSo { get; }

        public abstract void OnButtonClicked();

        public abstract void SetElementValue(UnitSO currSo, IPlaceable placeable);

        public void ButtonActivation(bool isActive)
        {
            if (button != null)
                button.interactable = isActive;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ButtonActivation(true);
        }
    }
}