using System.Collections.Generic;
using StrategyDemo.Logic;
using UnityEngine;

namespace StrategyDemo.Interfaces
{
    public interface IPlaceable
    {
        public GameObject Object { get; }
        public Vector2Int Size { get; }
        public List<CellInfo> PlaceableCellList{ get; }

        public void Place(List<CellInfo> cellList);
        public CellInfo GetPlaceableNeighbour();
    }
}