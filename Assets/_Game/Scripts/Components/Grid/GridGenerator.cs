using System;
using PanteonDemo.Logic;
using PanteonDemo.SO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PanteonDemo
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private GridSO gridSO;
        [SerializeField] private Grid grid;
        [SerializeField] private Transform cellParent;
        [SerializeField] private Transform cellBeginingPoint;

        private CellInfo[,] _cellArray;
        private Vector2Int _cellSize;
        private Vector2 _gridCellSize;
        private GameObject _cellPrefab;

        private void Start()
        {
            GenerateCell();
        }

        public void GetData()
        {
            _cellSize = gridSO.CellSize;
            _cellPrefab = gridSO.CellPrefab;
            _gridCellSize = gridSO.GridCellSize;
            grid.cellSize = _gridCellSize;
            CellInfo[,] array = gridSO.CellArray;
            if (array != null)
                _cellArray = array;
        }

        Vector3 GetWorldToCellsWorldPosition(Vector3 position)
        {
            Vector3Int cellPos = grid.WorldToCell(position);
            return grid.CellToWorld(cellPos);
        }

        private void SetCell()
        {
            _cellArray = new CellInfo[_cellSize.x, _cellSize.y];
            Vector3 currPos = cellBeginingPoint.position;

            for (int y = 0; y < _cellSize.y; y++)
            {
                for (int x = 0; x < _cellSize.x; x++)
                {
                    Vector3 cellPosition = GetWorldToCellsWorldPosition(currPos);
                    currPos = cellPosition;
                    cellPosition += (Vector3) (_gridCellSize * .5f);

                    GameObject newObject = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, cellParent);

                    _cellArray[x, y] = new CellInfo()
                    {
                        CenterPosition = cellPosition,
                        BeginingPosition = currPos,
                        Index = new Vector2Int(x, y)
                    };
                    currPos.x += _gridCellSize.x;
                }

                currPos.x = cellBeginingPoint.position.x;
                currPos.y += _gridCellSize.y;
            }

            gridSO.SetCellArray(_cellArray);
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

            if (!Application.isPlaying)
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
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
    }
}