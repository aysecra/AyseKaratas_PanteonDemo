using System.Collections.Generic;
using UnityEngine;


namespace PanteonDemo.Logic
{
    public class CellInfo 
    {
        public Vector3 BeginingPosition;
        public Vector3 CenterPosition;
        public Vector2Int Index;
        public CellInfo Connection;

        // distance from the cell to the start cell 
        public float G;

        // distance from the cell to the target cell 
        public float H;
        public float F => G + H;
        public bool IsWalkable = true;

        public List<CellInfo> Neighbors;

        public float GetDistance(CellInfo neighbor)
        {
            return Vector3.Distance(CenterPosition, neighbor.CenterPosition);
        }
    }
}
