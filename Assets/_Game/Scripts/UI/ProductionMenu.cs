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
            amountToPool = (uint) SharedLevelManager.Instance.BuildingElements.Count;
            base.Start();
            AddButtons();
        }

        private void AddButtons()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                Building building = SharedLevelManager.Instance.BuildingElements[i];
                AddButton(building);
            }
        }

        private void AddButton(Building building)
        {
            BuildingButton newBuildingButton = (BuildingButton) GetPooledObject();
            newBuildingButton.gameObject.SetActive(true);
            _scrollerElements.Add(newBuildingButton);
            newBuildingButton.SetElementValue(building);
        }
    }
}
