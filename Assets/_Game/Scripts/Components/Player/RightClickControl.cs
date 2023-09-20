using System.Collections.Generic;
using PanteonDemo.Enum;
using PanteonDemo.Event;
using PanteonDemo.Interfaces;
using PanteonDemo.Logic;
using PanteonDemo.Manager;
using UnityEngine;

namespace PanteonDemo.Component
{
    public class RightClickControl : Singleton<RightClickControl>
        , EventListener<InputEvent>
    {
        private GameObject _movableObject;
        private IMovableWithPath _movableObjectScript;
        private Camera _mainCamera;

        public bool IsClickable { get; set; }

        public GameObject MovableObject
        {
            get => _movableObject;
            set => _movableObject = value;
        }

        public IMovableWithPath MovableObjectScript
        {
            get => _movableObjectScript;
            set => _movableObjectScript = value;
        }

        private void Start()
        {
            IsClickable = true;
            _mainCamera = Camera.main;
        }

        public void DetectMovableAndTargetObject()
        {
            GridCollider gridCollider = null;
            GridGenerator gridGenerator = null;
            IDamageable damageableObjectScript = null;
            GameObject damageableObject = null;

            Vector2 currentMousePoint = InputManager.Instance.CurrentPosition;
            Vector3 origin = _mainCamera.ScreenToWorldPoint(currentMousePoint);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                Transform hitTransform = hit.transform;

                // if (_movableObject == null)
                // {
                //     if (hitTransform.TryGetComponent(out IMovableWithPath movableObject))
                //     {
                //         _movableObject = hitTransform.gameObject;
                //         _movableObjectScript = movableObject;
                //         movableObject.SetIsSelectedObject(true);
                //         break;
                //     }
                // }

                if (hitTransform.TryGetComponent(out GridCollider grid))
                {
                    gridCollider = grid;
                    gridGenerator = grid.GridGenerator;
                }

                else if (hitTransform.TryGetComponent(out IDamageable damageable))
                {
                    if (!_movableObject.Equals(hitTransform.gameObject))
                    {
                        damageableObjectScript = damageable;
                        damageableObject = hitTransform.gameObject;
                    }
                }
            }

            Move(gridCollider, gridGenerator, origin, damageableObjectScript, damageableObject);
        }

        void Move(GridCollider gridCollider, GridGenerator gridGenerator, Vector3 origin,
            IDamageable damageableObjectScript, GameObject damageableObject)
        {
            if (_movableObject != null && gridCollider != null && gridGenerator != null)
            {
                CellInfo startCell = gridGenerator.GetCellInfoToWorldPosition(_movableObject.transform.position);
                origin.z = gridCollider.transform.position.z;
                CellInfo targetCell = null;
                bool isTargetNeighbour = false;

                if (damageableObjectScript == null)
                    targetCell = gridGenerator.GetCellInfoToWorldPosition(origin);

                else if (damageableObject.TryGetComponent(out IPlaceable placeable))
                {
                    // control is damagable object neighbour
                    foreach (var neighbor in startCell.Neighbors)
                    {
                        foreach (var damagableCell in placeable.PlaceableCellList)
                        {
                            // print("target neighbor: " + neighbor.Index + " --> clicked" + damagableCell.Index);
                            if (neighbor.Index == damagableCell.Index)
                            {
                                isTargetNeighbour = true;
                                LeftClickControl.Instance.IsClickable = true;
                                IsClickable = true;
                                _movableObjectScript.SetIsSelectedObject(false);
                                damageableObjectScript.TakeDamage(_movableObject.GetComponent<IHitable>().Damage);
                                break;
                            }
                        }

                        if (isTargetNeighbour)
                            break;
                    }

                    if (!isTargetNeighbour)
                    {
                        targetCell = placeable.GetPlaceableNeighbour();
                    }
                }


                if (!isTargetNeighbour && targetCell != null)
                {
                    List<CellInfo> path = Pathfinding.FindPath(startCell, targetCell);
                    if (path is {Count: > 0})
                    {
                        startCell.IsWalkable = true;
                        path[^1].IsWalkable = false;

                        _movableObjectScript.GoPath(GetPathPositionArray(path), new List<CellInfo> {path[^1]},
                            damageableObjectScript);
                    }
                }

                _movableObjectScript.SetIsSelectedObject(false);
                _movableObject = null;
                _movableObjectScript = null;
            }
        }

        Vector3[] GetPathPositionArray(List<CellInfo> path)
        {
            Vector3[] result = new Vector3[path.Count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = path[i].CenterPosition;
            }

            return result;
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<InputEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<InputEvent>(this);
        }

        public void OnEventTrigger(InputEvent currentEvent)
        {
            if (IsClickable && currentEvent.State == TouchState.RightClick)
            {
                DetectMovableAndTargetObject();
            }

            if (currentEvent.State == TouchState.LeftClick)
            {
                if (_movableObjectScript != null && _movableObject != null)
                {
                    _movableObjectScript.SetIsSelectedObject(false);
                    _movableObjectScript = null;
                    _movableObject = null;
                }
            }
        }
    }
}