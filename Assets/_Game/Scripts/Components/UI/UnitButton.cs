using StrategyDemo.Interfaces;
using StrategyDemo.SO;
using UnityEngine;
using UnityEngine.UI;

namespace StrategyDemo.Component
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
            if (!ReferenceEquals(button, null))
                button.interactable = isActive;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ButtonActivation(true);
        }
    }
}