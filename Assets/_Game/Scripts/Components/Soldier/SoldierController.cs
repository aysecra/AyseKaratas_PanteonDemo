using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class SoldierController : PoolableObject
    {
        [Header("Core Elements")] [SerializeField]
        private List<SoldierType> _soldierTypes = new List<SoldierType>();

        [SerializeField] private Image _healthbar;
        [SerializeField] private GameObject _soldierClickArea;

        [Header("Selectable Elements")] [SerializeField, StringInList(typeof(PropertyDrawersHelper), "AllSoldierNames")]
        private string _currentSoldierName;

        [SerializeField] private float _durationPerCell = .25f;

        private SoldierData _currentSoldierData;

        public GridsCell PlacedCell { get; set; }
        public SoldierData CurrentSoldierData => _currentSoldierData;
        private int _currentHealth;

        private void Start()
        {
            CloseClickedArea();
            SetType(_currentSoldierName);
        }

        private void OnValidate()
        {
            SetSoldierType(_currentSoldierName);
        }

        public void OpenClickedArea()
        {
            _soldierClickArea.SetActive(true);
        }
        
        public void CloseClickedArea()
        {
            _soldierClickArea.SetActive(false);
        }

        public void SetType(string soldierName)
        {
            _currentSoldierName = soldierName;
            SetSoldierType(soldierName);
            SetCurrentSoldier();
        }

        private void SetSoldierType(string soldierName)
        {
            foreach (SoldierType soldierType in _soldierTypes)
            {
                soldierType.SoldierObject.SetActive(soldierName == soldierType.Name);
            }
        }

        // set soldierData type
        private void SetCurrentSoldier()
        {
            foreach (var soldier in SharedLevelManager.Instance.SoldierUnits)
            {
                if (soldier.Name == _currentSoldierName)
                {
                    _currentSoldierData = soldier;
                    _currentHealth = (int) _currentSoldierData.Health;
                    _healthbar.fillAmount = (float) _currentHealth / _currentSoldierData.Health;
                    break;
                }
            }
        }

        // move to empty cell
        public void Move(Vector3[] path, GridsCell targetCell)
        {
            transform.DOPath(path, _durationPerCell * path.Length)
                .OnStart((() => { PlayerController.Instance.IsClickable = false; }))
                .OnComplete((() =>
                {
                    PlayerController.Instance.IsClickable = true;
                    PlacedCell.CellBase.IsWalkable = true;
                    targetCell.CellBase.IsWalkable = false;
                    PlacedCell = targetCell;
                    CloseClickedArea();
                }));
        }

        // move to buildingData or soldierData
        public void Move<T>(Vector3[] path, GridsCell targetCell, T element) where T : class
        {
            transform.DOPath(path, _durationPerCell * path.Length)
                .OnStart((() => { PlayerController.Instance.IsClickable = false; }))
                .OnComplete((() =>
                {
                    PlayerController.Instance.IsClickable = true;
                    PlacedCell.CellBase.IsWalkable = true;
                    targetCell.CellBase.IsWalkable = false;
                    PlacedCell = targetCell;
                    CloseClickedArea();
                    HitToElement(element);
                }));
        }

        // hit to buildingData or soldierData
        private void HitToElement<T>(T element) where T : class
        {
            if (typeof(T) == typeof(SoldierController))
            {
                (element as SoldierController).TakeDamage((int) _currentSoldierData.Damage);
            }

            else if (typeof(T) == typeof(BuildingController))
            {
                (element as BuildingController).TakeDamage((int) _currentSoldierData.Damage);
            }
        }

        public void TakeDamage(int damage)
        {
            _currentHealth = _currentHealth - damage > 0 ? _currentHealth - damage : 0;
            _healthbar.fillAmount = (float) _currentHealth / _currentSoldierData.Health;
            if(_currentHealth == 0)
            {
                gameObject.SetActive(false);
                PlacedCell.CellBase.IsWalkable = true;
            }
        }
    }
}