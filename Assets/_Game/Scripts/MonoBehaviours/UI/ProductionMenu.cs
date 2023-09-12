using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class ProductionMenu : ObjectPool
    {
        [Header("Core Elements")] [SerializeField]
        private Scrollbar _scrollbar;
        
        private int _characterCount;
        private List<BuildingButton> _scrollerElements = new List<BuildingButton>();
        private bool _isBeginScrollbar;
        
        protected override void Start()
        {
            // get amount to pool from shared level manager
            amountToPool = (uint) SharedLevelManager.Instance.BuildingElements.Count;
            // set pool
            base.Start();
            // open buttons from pool
            AddButtons();
        }
        
        private void Update()
        {
            // set scroller to begining point
            if (!_isBeginScrollbar && _scrollbar.value != 1)
            {
                _scrollbar.value = 1;
                _isBeginScrollbar = true;
            }
        }

        // open multiple button
        private void AddButtons()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                Building building = SharedLevelManager.Instance.BuildingElements[i];
                AddButton(building);
            }
        }

        // open a building spawn button
        private void AddButton(Building building)
        {
            BuildingButton newBuildingButton = (BuildingButton) GetPooledObject();
            newBuildingButton.gameObject.SetActive(true);
            _scrollerElements.Add(newBuildingButton);
            newBuildingButton.SetElementValue(building);
        }
    }
}
