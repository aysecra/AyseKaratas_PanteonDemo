using System.Collections.Generic;
using PanteonDemo.Logic;
using UnityEngine;

namespace PanteonDemo.Interfaces
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