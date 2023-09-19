using System.Collections.Generic;
using PanteonDemo.Logic;
using UnityEngine;

namespace PanteonDemo.Interfaces
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
