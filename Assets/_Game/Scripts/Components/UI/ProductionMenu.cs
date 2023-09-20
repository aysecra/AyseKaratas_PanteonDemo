using System.Collections;
using System.Collections.Generic;
using PanteonDemo.SO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PanteonDemo.Component
{
    public class ProductionMenu : InfiniteScroller
    {
        [SerializeField] private UnitySO unitySo;
        [SerializeField] private VerticalLayoutGroup verticalLayout;
        [SerializeField] private ContentSizeFitter contentSizeFitter;
        [SerializeField] private int maxNotScrollingAmount = 20;

        private int _characterCount;
        private List<UnitButton> _scrollerElements = new List<UnitButton>();
        private bool _isBeginScrollbar;
        private int lastUnitIndex;
        private int firstUnitIndex;

        protected override void Start()
        {
            // set pool
            base.Start();
            // open buttons from pool
            AddButtons();
        }

        // open multiple button
        private void AddButtons()
        {
            List<UnitSO> unitList = unitySo.UnitList;
            int maxCount = AmountToPool > unitList.Count ? unitList.Count : (int) AmountToPool;
            if (unitList.Count < maxNotScrollingAmount) scrollRect.enabled = false;

            for (int i = 0; i < maxCount; i++)
            {
                BuildingUnitSO buildingSO = (BuildingUnitSO) unitList[i];
                AddButton(buildingSO);
            }

            firstUnitIndex = maxCount - 1;
            lastUnitIndex = 0;

            _childCount = maxCount;
            StartCoroutine(CloseComponents());
        }
        
        protected override void OnValueChanged(Vector2 other)
        {
            if (!_isStart)
            {
                _isStart = true;
                _lastPos = other;
            }

            Vector2 pos = other;
            _isPositiveDirection = _lastPos.y > pos.y;
            _lastPos = pos;

            int currentItemIndex = _isPositiveDirection ? _childCount - 1 : 0;
            var currentItem = scrollRect.content.GetChild(currentItemIndex);

            if (!IsReachedThreshold(currentItem))
            {
                return;
            }
            
            
            int endItemIndex = _isPositiveDirection ? 0 : _childCount - 1;
            Transform endItem = scrollRect.content.GetChild(endItemIndex);
            Vector2 newPosition = endItem.position;

            if (_isPositiveDirection)
            {
                newPosition.y = endItem.position.y - elementSize.y * 1.5f - elementSpace;
            }
            else
            {
                newPosition.y = endItem.position.y + elementSize.y * 1.5f + elementSpace;
            }

            currentItem.position = newPosition;
            currentItem.SetSiblingIndex(endItemIndex);
            
            if (currentItem.TryGetComponent(out BuildingButton buildingButton))
            {
                List<UnitSO> unitList = unitySo.UnitList;
            
                if (_isPositiveDirection)
                {
                    buildingButton.SetElementValue((BuildingUnitSO) unitList[lastUnitIndex]);
                    lastUnitIndex = lastUnitIndex + 1 < unitList.Count ? lastUnitIndex + 1 : 0;
                }
                else
                {
                    buildingButton.SetElementValue((BuildingUnitSO) unitList[firstUnitIndex]);
                    firstUnitIndex = firstUnitIndex - 1 >= 0 ? firstUnitIndex - 1 : unitList.Count - 1;
                }
            }
        }
        
        IEnumerator CloseComponents()
        {
            yield return new WaitForSeconds(.5f);
            verticalLayout.enabled = false;
            contentSizeFitter.enabled = false;
        }

        // open a building button spawn button
        private void AddButton(BuildingUnitSO buildingSO)
        {
            BuildingButton newBuildingButton = (BuildingButton) GetPooledObject();
            newBuildingButton.gameObject.SetActive(true);
            _scrollerElements.Add(newBuildingButton);
            newBuildingButton.SetElementValue(buildingSO);
        }

        public void AllButtonActivation(bool isActive)
        {
            foreach (var element in _scrollerElements)
            {
                element.ButtonActivation(isActive);
            }
        }
    }
}