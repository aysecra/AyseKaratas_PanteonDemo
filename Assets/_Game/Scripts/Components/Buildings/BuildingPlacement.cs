using System.Collections.Generic;
using PanteonDemo.Interfaces;
using PanteonDemo.Logic;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Component
{
    public class BuildingPlacement : MonoBehaviour, IPlaceable
        , IDragable
    {
        [SerializeField] private GridSO placedGridSo;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private SpriteRenderer controlPlacementArea;
        [SerializeField] private Color placeableColor;
        [SerializeField] private Color notplaceableColor;

        private Vector2Int _size;
        private bool _isDragable;
        private bool _isPlaceable;
        private List<CellInfo> _placeableCellList = new List<CellInfo>();

        public GameObject Object => gameObject;
        public Vector2Int Size => _size;
        public bool ISPlaceable => _isPlaceable;
        public List<CellInfo> PlaceableCellList => _placeableCellList;

        public void SetPlaceable()
        {
            _isPlaceable = true;
            controlPlacementArea.color = placeableColor;
        }

        public void SetNotPlaceable()
        {
            _isPlaceable = false;
            controlPlacementArea.color = notplaceableColor;
        }

        public void SetPosition(Vector3 position, List<CellInfo> placeableCellList)
        {
            _placeableCellList = placeableCellList;
            transform.position = position;
        }

        public void SetBuilding(Vector2Int size)
        {
            _size = size;
            Vector3 gridCellSize = placedGridSo.GridCellSize;
            Vector2 scale = new Vector2(_size.x * gridCellSize.x, _size.y * gridCellSize.y);
            boxCollider.size = scale;
            controlPlacementArea.transform.localScale = scale;
        }

        public void Place(List<CellInfo> cellList)
        {
            controlPlacementArea.gameObject.SetActive(false);
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