using PanteonDemo.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace PanteonDemo.Component
{
    public class GridCollider : MonoBehaviour
    {
        [SerializeField] private GridSO gridSO;
        [SerializeField] private GridGenerator gridGenerator;

        private Vector3 downLeft;
        private Vector3 upRight;
        public GridGenerator GridGenerator => gridGenerator;

        public void SetCollider()
        {
            Vector2 size = CalculateColliderSize();
            transform.localScale = size;
            downLeft = gridSO.GridDownLeftPosition;
            upRight = new Vector3(downLeft.x + size.x, downLeft.y + size.y, transform.position.z);
            transform.position = CalculateCenterOfCollider(downLeft, size);

            #if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            #endif
        }

        private Vector2 CalculateColliderSize()
        {
            Vector2 cellSize = gridSO.CellSize;
            Vector2 gridSize = gridSO.GridCellSize;
            return new Vector2(cellSize.x * gridSize.x, cellSize.y * gridSize.y);
        }

        private Vector3 CalculateCenterOfCollider(Vector3 downLeftPos, Vector2 size)
        {
            return new Vector3(downLeftPos.x + size.x * .5f, downLeftPos.y + size.y * .5f, transform.position.z);
        }

        public bool IsItWithinBorders(Vector3 position, out Vector2Int overflowDirection)
        {
            Vector2 size = CalculateColliderSize();
            downLeft = gridSO.GridDownLeftPosition;
            upRight = new Vector3(downLeft.x + size.x, downLeft.y + size.y, transform.position.z);
            transform.position = CalculateCenterOfCollider(downLeft, size);
            
            overflowDirection = Vector2Int.zero;

            overflowDirection += position.x < downLeft.x ? new Vector2Int(-1, 0) : Vector2Int.zero;
            overflowDirection += position.y < downLeft.y ? new Vector2Int(0, -1) : Vector2Int.zero;

            if (overflowDirection != Vector2Int.zero) return false;

            overflowDirection += position.x > upRight.x ? new Vector2Int(1, 0) : Vector2Int.zero;
            overflowDirection += position.y > upRight.y ? new Vector2Int(0, 1) : Vector2Int.zero;
            
            return overflowDirection == Vector2Int.zero;
        }
    }
}