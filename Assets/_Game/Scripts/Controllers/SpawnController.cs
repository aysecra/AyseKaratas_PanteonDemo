using System;
using System.Collections.Generic;
using PanteonDemo.Component;
using PanteonDemo.Event;
using PanteonDemo.Interfaces;
using PanteonDemo.Logic;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Controller
{
    public class SpawnController : MonoBehaviour, EventListener<SpawnEvent>
    {
        [SerializeField] private GridGenerator _spawnArea;

        public void OnEventTrigger(SpawnEvent currentEvent)
        {
            UnitSO currUnit = currentEvent.Unit;
            PoolableObject spawnedObject = SharedLevelManager.Instance.GetObject(currUnit);
            spawnedObject.gameObject.SetActive(true);
            ((IChangeable) spawnedObject).SetType(currUnit);
            List<CellInfo> cellArea = _spawnArea.GetSpawnArea(currUnit.Size, out Vector3 spawnPos);
            
            if (cellArea.Count > 0)
            {
                if (spawnedObject.TryGetComponent(out IDragable dragable))
                {
                    dragable.SetPosition(spawnPos, cellArea);
                    LeftClickControl.Instance.SpawnedDragableObject = dragable;
                }

                else
                {
                    if (currentEvent.SpawnerPlaceable == null)
                    {
                        spawnedObject.transform.position = spawnPos;
                        PlacementController.Place(spawnedObject.GetComponent<IPlaceable>(), cellArea);
                    }
                    else
                    {
                        CellInfo target = currentEvent.SpawnerPlaceable.GetPlaceableNeighbour();
                        
                        if (target != null)
                        {
                            spawnedObject.transform.position = target.CenterPosition;
                            PlacementController.Place(spawnedObject.GetComponent<IPlaceable>(),
                                new List<CellInfo>() {currentEvent.SpawnerPlaceable.GetPlaceableNeighbour()});
                        }
                        else
                        {
                            spawnedObject.transform.position = spawnPos;
                            PlacementController.Place(spawnedObject.GetComponent<IPlaceable>(), cellArea);
                        }
                    }
                }
            }
            else
            {
                spawnedObject.gameObject.SetActive(false);
                GUIManager.Instance.ObjectNotSpawnable();
            }
        }

        private void OnEnable()
        {
            EventManager.EventStartListening(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening(this);
        }
    }
}