using System;
using PanteonDemo.Logic;
using UnityEngine;

namespace PanteonDemo.SO
{
    [CreateAssetMenu(menuName = "SO/Grid Element")]
    public class GridSO : ScriptableObject
    {
        [SerializeField] private Vector2Int cellSize = Vector2Int.one;
        [SerializeField] private Vector2 gridCellSize = Vector2.one;
        [SerializeField] private GameObject cellPrefab;

        private CellInfo[,] _cellArray;
        [SerializeField] private Vector3 _gridDownLeftPosition; 

        public Vector2Int CellSize => cellSize;

        public GameObject CellPrefab => cellPrefab;

        public Vector2 GridCellSize => gridCellSize;
        public Vector3 GridDownLeftPosition => _gridDownLeftPosition;
        
        public CellInfo[,] CellArray
        {
            get
            {
                if (_cellArray != null)
                {
                    CellInfo[,] result = new CellInfo[_cellArray.GetLength(0), _cellArray.GetLength(1)];
                    Array.Copy(_cellArray, result, _cellArray.Length);
                    return result;
                }

                return null;
            }
        }
        
        public void ClearCell()
        {
            _cellArray = null;
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void SetCellArray(CellInfo[,] array)
        {
            _cellArray = new CellInfo[array.GetLength(0), array.GetLength(1)];
            Array.Copy(array, _cellArray, array.Length);

            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void SetGridDownLeftPosition(Vector3 position)
        {
            _gridDownLeftPosition = position;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    }
}