using System.Collections.Generic;
using StrategyDemo.Logic;
using UnityEngine;

namespace StrategyDemo.Interfaces
{
    public interface IDragable
    {
        public GameObject Object { get; }

        public Vector2Int Size { get; }
        public bool ISPlaceable { get; }
        public List<CellInfo> PlaceableCellList { get; }

        public void SetPlaceable();
        public void SetNotPlaceable();
        public void SetPosition(Vector3 position, List<CellInfo> placeableCellList);
    }
}
