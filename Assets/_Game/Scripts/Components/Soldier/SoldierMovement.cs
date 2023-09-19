using System;
using System.Collections.Generic;
using DG.Tweening;
using PanteonDemo.Controller;
using PanteonDemo.Interfaces;
using PanteonDemo.Logic;
using UnityEngine;

namespace PanteonDemo.Component
{
    public class SoldierMovement : MonoBehaviour
        , IMovableWithPath
        , IPlaceable
    {
        [SerializeField] private GameObject _selectedObjectArea;

        private float _durationPerCell;
        private Vector2Int _size;
        private List<CellInfo> _placeableCellList;

        public GameObject Object => gameObject;
        public Vector2Int Size => _size;
        public List<CellInfo> PlaceableCellList => _placeableCellList;

        public void SetSoldier(float duration, Vector2Int size)
        {
            _durationPerCell = duration;
            _size = size;
        }

        public void GoPath(Vector3[] path, List<CellInfo> targetCell, IDamageable damageable = null)
        {
            transform.DOPath(path, _durationPerCell * path.Length)
                .OnStart((() =>
                {
                    LeftClickControl.Instance.IsClickable = false;
                    RightClickControl.Instance.IsClickable = false;
                }))
                .OnComplete((() =>
                {
                    LeftClickControl.Instance.IsClickable = true;
                    RightClickControl.Instance.IsClickable = true;
                    PlacementController.Place(this, targetCell);
                    SetIsSelectedObject(false);
                    if(damageable != null)
                        damageable.TakeDamage(GetComponent<IHitable>().Damage);
                }));
        }

        public void SetIsSelectedObject(bool isActive)
        {
            _selectedObjectArea.SetActive(isActive);
        }

        public void Place(List<CellInfo> cellList)
        {
            _placeableCellList = cellList;
            foreach (var cell in cellList)
            {
                cell.IsWalkable = false;
            }
        }

        public CellInfo GetPlaceableNeighbour()
        {
            // control empty cell 
            CellInfo targetCell = null;

            foreach (CellInfo cell in _placeableCellList)
            {
                foreach (var neighbour in cell.Neighbors)
                {
                    if (neighbour.IsWalkable)
                    {
                        targetCell = neighbour;
                        break;
                    }
                }

                if (targetCell != null)
                    break;
            }

            return targetCell;
        }
    }
}