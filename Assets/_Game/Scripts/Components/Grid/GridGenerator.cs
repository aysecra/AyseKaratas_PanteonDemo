using System;
using System.Collections.Generic;
using PanteonDemo.Logic;
using PanteonDemo.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace PanteonDemo.Component
{
    public abstract class GridGenerator : MonoBehaviour
    {
        [SerializeField] protected GridSO gridSO;
        [SerializeField] private Grid grid;
        [SerializeField] protected Transform cellParent;
        [SerializeField] protected Transform cellDownLeftPoint;
        [SerializeField] protected GridCollider gridCollider;

        protected CellInfo[,] _cellArray;
        protected Vector2Int _cellSize;
        protected Vector2 _gridCellSize;
        protected GameObject _cellPrefab;

        protected abstract void SetCell();

        protected abstract void SetNeigbors();

        public abstract void CalculateObjectCenter(Vector3 position, Vector2Int size, out Vector3 centerPos,
            out List<CellInfo> cellList);

        public abstract bool CalculateObjectFromUpRightCenter(Vector3 position, Vector2Int size, out Vector3 centerPos,
            out List<CellInfo> cellList);

        public abstract Vector3 GetWorldToCellsCenterPosition(Vector3 position);
        public abstract List<CellInfo> GetSpawnArea(Vector2Int size, out Vector3 position);
        protected abstract Vector3 CalculateObjectCenter(Vector3 position, Vector2Int size);
        public abstract Vector3 GetRightUpOrigin(Vector3 origin, Vector2Int size);


        private void Start()
        {
            GenerateCell();
        }

        void GetData()
        {
            _cellSize = gridSO.CellSize;
            _cellPrefab = gridSO.CellPrefab;
            _gridCellSize = gridSO.GridCellSize;
            grid.cellSize = _gridCellSize;
            CellInfo[,] array = gridSO.CellArray;
            if (array != null)
                _cellArray = array;
            else
            {
                ClearCell();
                SetCell();
            }
        }

        protected Vector3 GetWorldToCellsWorldPosition(Vector3 position)
        {
            Vector3Int cellPos = grid.WorldToCell(position);
            return grid.CellToWorld(cellPos);
        }

        public void GenerateCell()
        {
            GetData();

            int cellAmount = cellParent.childCount;

            if (cellAmount == 0)
            {
                SetCell();
            }

            else if (cellAmount != _cellSize.x * _cellSize.y)
            {
                ClearCell();
                SetCell();
            }

            #if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            #endif
        }

        public void ClearCell()
        {
            while (cellParent.childCount > 0)
            {
                foreach (Transform child in cellParent)
                {
                    DestroyImmediate(child.gameObject);
                }

                gridSO.ClearCell();
            }

            _cellArray = new CellInfo[0, 0];
            GC.Collect();
        }

        public CellInfo GetCell(Vector2Int index)
        {
            return _cellArray[index.x, index.y];
        }

        public CellInfo GetCellInfoToWorldPosition(Vector3 position)
        {
            Vector3 pos = GetWorldToCellsCenterPosition(position);

            foreach (CellInfo cell in _cellArray)
            {
                if (pos == cell.CenterPosition)
                {
                    return cell;
                }
            }

            return null;
        }
    }
}