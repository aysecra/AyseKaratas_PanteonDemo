using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class InfiniteScrollerController : ObjectPool
    {
        [Header("Core Elements")] [SerializeField]
        private Transform _elementContainer;

        [SerializeField] private ScrollerElement _scrollerElement;
        [SerializeField] private Scrollbar _scrollbar;

        private int _characterCount;
        private List<ScrollerElement> _scrollerElements = new List<ScrollerElement>();
        private bool _isBeginScrollbar;

        protected override void Start()
        {
            amountToPool = (uint) SharedLevelManager.Instance.SpawnedSoldierElements.Count;
            base.Start();
            AddButtons();
        }

        private void AddButtons()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                Soldier soldier = SharedLevelManager.Instance.SpawnedSoldierElements[i];
                AddButton(soldier);
            }
        }

        private void AddButton(Soldier soldier)
        {
            GameObject newObject = Instantiate(_scrollerElement.gameObject, _elementContainer);
            if (newObject.TryGetComponent(out ScrollerElement scrollerElement))
            {
                _scrollerElements.Add(scrollerElement);
                scrollerElement.SetElementValue(soldier);
            }
        }
    }
}