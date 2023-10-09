using System.Collections.Generic;
using StrategyDemo.Logic;
using StrategyDemo.Interfaces;
using StrategyDemo.SO;
using UnityEngine;

namespace StrategyDemo.Event
{
    /// <summary>
    /// Event is triggered when a unit needs to spawn
    /// </summary>
    public struct SpawnEvent
    {
        public UnitSO Unit{ get; private set; }
        public IPlaceable SpawnerPlaceable { get; private set; }

        public SpawnEvent(UnitSO unitSo, IPlaceable spawnerPlaceable = null)
        {
            Unit = unitSo;
            SpawnerPlaceable = spawnerPlaceable;
        }
    }
}