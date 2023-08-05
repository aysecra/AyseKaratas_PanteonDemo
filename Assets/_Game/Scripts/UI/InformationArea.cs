using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class InformationArea : ObjectPool
    {
        [Header("Core Elements")] [SerializeField]
        private Scrollbar _scrollbar;

        private int _characterCount;
        private List<SoldierButton> _scrollerElements = new List<SoldierButton>();
        private bool _isBeginScrollbar;

        protected override void Start()
        {
            amountToPool = (uint) SharedLevelManager.Instance.SoldierUnits.Count;
            base.Start();
            AddButtons();
        }

        private void AddButtons()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                Soldier soldier = SharedLevelManager.Instance.SoldierUnits[i];
                AddButton(soldier);
            }
        }

        private void AddButton(Soldier soldier)
        {
            SoldierButton newSoldierButton = (SoldierButton) GetPooledObject();
            newSoldierButton.gameObject.SetActive(true);
            _scrollerElements.Add(newSoldierButton);
            newSoldierButton.SetElementValue(soldier);
        }
    }
}