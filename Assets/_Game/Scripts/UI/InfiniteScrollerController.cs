using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class InfiniteScrollerController : ObjectPool
    {
        [Header("Core Elements")]
        [SerializeField] private Transform _elementContainer;
        [SerializeField] private ScrollerElement _scrollerElement;
        [SerializeField] private Scrollbar _scrollbar;
        
        private int _characterCount;
        private List<ScrollerElement> _scrollerElements = new List<ScrollerElement>();
        private bool _isBeginScrollbar;

        private void Awake()
        {
            amountToPool = (uint)SharedLevelManager.Instance.SpawnedSoldierElements.Count;
        }

        protected override void Start()
        {
            base.Start();
            
        }


        private void AddButton(int i)
        {
            GameObject newObject = Instantiate(_scrollerElement.gameObject, _elementContainer);
            if (newObject.TryGetComponent(out ScrollerElement scrollerElement))
            {
                _scrollerElements.Add(scrollerElement);
                // scrollerElement.SetElementValue();
            }
        }

    }
}
