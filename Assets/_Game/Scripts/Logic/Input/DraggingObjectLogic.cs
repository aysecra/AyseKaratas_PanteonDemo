using System.Collections.Generic;
using PanteonDemo.Component;
using PanteonDemo.Interfaces;
using PanteonDemo.Manager;
using UnityEngine;

namespace PanteonDemo.Logic
{
    public static class DraggingObjectLogic
    {
        public static bool IsTouchStart;
        
        public static void DetectAndDragObject(Camera mainCamera, IDragable spawnedDragableObject)
        {
            // 1: cell, 2: touched draggable object, 3: placed object (max version)
            int hitCounter = 3;
            bool isPlaceable = true;
            GridGenerator gridGenerator = null;

            Vector2 currentMousePoint = InputManager.Instance.CurrentPosition;
            Vector3 origin = mainCamera.ScreenToWorldPoint(currentMousePoint);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero);
            GridCollider gridCollider = null;

            foreach (RaycastHit2D hit in hits)
            {
                Transform hitTransform = hit.transform;

                if (hitTransform.TryGetComponent(out gridCollider))
                {
                    origin.z = gridCollider.transform.position.z;
                    gridGenerator = gridCollider.GridGenerator;
                    hitCounter -= 1;
                }

                else if (hitTransform.TryGetComponent(out IPlaceable placeable))
                {
                    if (!spawnedDragableObject.Object.Equals(placeable.Object))
                    {
                        spawnedDragableObject.SetNotPlaceable();
                        isPlaceable = false;
                    }
                    else
                    {
                        if (!IsTouchStart)
                        {
                            RightClickControl.Instance.IsClickable = false;
                            IsTouchStart = true;
                        }
                    }

                    hitCounter -= 1;
                }

                if (hitCounter <= 0)
                {
                    break;
                }
            }

            if (IsTouchStart && gridCollider != null)
            {
                if (gridGenerator != null)
                {
                    gridGenerator.CalculateObjectCenter(origin, spawnedDragableObject.Size,
                        out Vector3 cellPos, out List<CellInfo> cellList);
                    spawnedDragableObject.SetPosition(cellPos, cellList);
                }

                if (isPlaceable)
                {
                    spawnedDragableObject.SetPlaceable();
                }
            }
        }
    }
}