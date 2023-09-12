using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class InformationArea : ObjectPool
    {
        [Header("Core Elements")] [SerializeField]
        private Scrollbar _scrollbar;

        [SerializeField] private Image _imgInfo;
        [SerializeField] private TMPro.TextMeshProUGUI _txtName;
        [SerializeField] private TMPro.TextMeshProUGUI _txtInfo;
        [SerializeField] private GameObject _productArea;

        private int _characterCount;
        private List<SoldierButton> _scrollerElements = new List<SoldierButton>();
        private bool _isBeginScrollbar;

        protected override void Start()
        {
            amountToPool = (uint) SharedLevelManager.Instance.SoldierUnits.Count;
            base.Start();
        }

        private void Update()
        {
            if (!_isBeginScrollbar && _scrollbar.value != 1)
            {
                _scrollbar.value = 1;
                _isBeginScrollbar = true;
            }
        }

        public void ClearInfoArea()
        {
            CloseAllButton();
            _txtName.gameObject.SetActive(false);
            _txtInfo.gameObject.SetActive(false);
            _imgInfo.gameObject.SetActive(false);
            _productArea.SetActive(false);
        }

        public void OpenInfoArea()
        {
            CloseAllButton();
            _txtName.gameObject.SetActive(true);
            _txtInfo.gameObject.SetActive(true);
            _imgInfo.gameObject.SetActive(true);
        }

        public void OpenInfo(Building infoElement, BuildingController selectedBuilding)
        {
            OpenInfoArea();
            _imgInfo.sprite = infoElement.Image;
            _txtInfo.text = $"{infoElement.Info}Health: {infoElement.Health}hp";
            _txtName.text = infoElement.Name;

            if (infoElement.Production != null && infoElement.Production.Count > 0)
            {
                List<Soldier> soldierList = new List<Soldier>();
                foreach (var production in infoElement.Production)
                {
                    Soldier soldier = SharedLevelManager.Instance.GetSoldier(production.Name);
                    if (soldier != null) soldierList.Add(soldier);
                }

                if (soldierList.Count > 0)
                {
                    _productArea.SetActive(true);
                    AddButtons(soldierList,selectedBuilding);
                }
                else
                {
                    _productArea.SetActive(false);
                }
            }
            else
            {
                _productArea.SetActive(false);
            }
        }

        public void OpenInfo(Soldier infoElement)
        {
            OpenInfoArea();
            _productArea.SetActive(false);
            _imgInfo.sprite = infoElement.Image;
            _txtInfo.text = $"{infoElement.Info} Health: {infoElement.Health} hp  Damage: {infoElement.Damage}";
            _txtName.text = infoElement.Name;
        }

        private void AddButtons(List<Soldier> productList, BuildingController selectedBuilding)
        {
            for (int i = 0; i < productList.Count; i++)
            {
                Soldier soldier = productList[i];
                AddButton(soldier,selectedBuilding);
            }
        }

        private void AddButton(Soldier soldier, BuildingController selectedBuilding)
        {
            SoldierButton newSoldierButton = (SoldierButton) GetPooledObject();
            newSoldierButton.gameObject.SetActive(true);
            _scrollerElements.Add(newSoldierButton);
            newSoldierButton.SetElementValue(soldier,selectedBuilding);
        }

        private void CloseAllButton()
        {
            foreach (SoldierButton soldierButton in _scrollerElements)
            {
                soldierButton.gameObject.SetActive(false);
            }

            _scrollerElements.Clear();
        }
    }
}