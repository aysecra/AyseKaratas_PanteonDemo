using System.Collections.Generic;
using StrategyDemo.Component;
using StrategyDemo.Interfaces;
using UnityEngine;

namespace StrategyDemo.Logic
{
    public class SquareCell : CellInfo, ICellShape
    {
        public void CalculateNeighbour(GridGenerator generator, Vector2Int size)
        {
            Neighbors = new List<CellInfo>();
            
            // get down neighbor
            if (Index.y > 0)
            {
                Neighbors.Add(generator.GetCell(Index + new Vector2Int(0, -1)));
            }

            // get up neighbor
            if (Index.y < size.y - 1)
            {
                Neighbors.Add(generator.GetCell(Index + new Vector2Int(0, 1)));
            }

            // get left neighbor
            if (Index.x > 0)
            {
                Neighbors.Add(generator.GetCell(Index + new Vector2Int(-1, 0)));
            }

            // get right neighbor
            if (Index.x < size.x - 1)
            {
                Neighbors.Add(generator.GetCell(Index + new Vector2Int(1, 0)));
            }
        }
    }
}