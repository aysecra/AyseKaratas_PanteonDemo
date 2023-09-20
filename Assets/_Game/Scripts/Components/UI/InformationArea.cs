using System.Collections.Generic;
using PanteonDemo.Interfaces;
using PanteonDemo.SO;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo.Component
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
        private bool _isBeginScrollbar;
        private List<SoldierButton> _productButtonList = new List<SoldierButton>();
        private IPlaceable _placeable;

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

        private void OpenInfoArea()
        {
            CloseAllButton();
            _txtName.gameObject.SetActive(true);
            _txtInfo.gameObject.SetActive(true);
            _imgInfo.gameObject.SetActive(true);
        }

        public void OpenInfo(UnitSO unitSo, IPlaceable placeable = null)
        {
            _placeable = placeable;
            OpenInfoArea();
            _imgInfo.sprite = unitSo.Image;
            SoldierUnitSO soldierSo = unitSo as SoldierUnitSO;
            if (soldierSo == null)
                _txtInfo.text = $"{unitSo.Info} Health:{unitSo.Health}hp";
            else
            {
                _txtInfo.text = $"{unitSo.Info} Health:{unitSo.Health}hp Damage:{soldierSo.Damage}";
            }

            _txtName.text = unitSo.Name;
            List<UnitSO> productList = null;

            if (unitSo.GetType() == typeof(BuildingUnitSO))
                productList = ((BuildingUnitSO) unitSo).ProductUnitList;

            if (productList != null && productList.Count > 0)
            {
                if (productList != null && productList.Count > 0)
                {
                    _productArea.SetActive(true);
                    AddButtons(productList);
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

        private void AddButtons(List<UnitSO> productList)
        {
            for (int i = 0; i < productList.Count; i++)
            {
                UnitSO product = productList[i];
                AddButton((SoldierUnitSO) product);
            }
        }

        private void AddButton(SoldierUnitSO soldierSO)
        {
            SoldierButton newSoldierButton = (SoldierButton) GetPooledObject();
            _productButtonList.Add(newSoldierButton);
            newSoldierButton.gameObject.SetActive(true);
            newSoldierButton.SetElementValue(soldierSO, _placeable);
        }

        private void CloseAllButton()
        {
            foreach (SoldierButton soldierButton in _productButtonList)
            {
                soldierButton.gameObject.SetActive(false);
            }

            _productButtonList.Clear();
        }
    }
}