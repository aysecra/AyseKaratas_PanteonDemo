using System;
using System.Collections.Generic;
using System.Linq;
using StrategyDemo.Logic;
using UnityEngine;

namespace StrategyDemo
{
    public static class Pathfinding
    {
        public static List<CellInfo> FindPath(CellInfo startCell, CellInfo targetCell)
        {
            List<CellInfo> toSearch = new List<CellInfo>(){startCell};
            List<CellInfo> processed = new List<CellInfo>();

            while (toSearch.Any())
            {
                CellInfo currentCell = toSearch[0];
                foreach (var searchCell in toSearch)
                {
                    if (searchCell.F <= currentCell.F && searchCell.H < currentCell.H)
                    {
                        currentCell = searchCell;
                    }
                }
                
                processed.Add(currentCell);
                toSearch.Remove(currentCell);

                if (currentCell == targetCell)
                {
                    CellInfo currentPathTile = targetCell;
                    List<CellInfo> path = new List<CellInfo>();

                    while (currentPathTile != startCell)
                    {
                        path.Add(currentPathTile);
                        currentPathTile = currentPathTile.Connection;
                    }

                    path.Reverse();
                    return path;
                }

                IEnumerable<CellInfo> walkableNeighbors =
                    currentCell.Neighbors.Where(cell => cell.IsWalkable && !processed.Contains((cell)));

                foreach (var neighbor in walkableNeighbors)
                {
                    bool inSearch = toSearch.Contains(neighbor);

                    float costToNeighbour = currentCell.G + currentCell.GetDistance(neighbor);

                    if (!inSearch || costToNeighbour < neighbor.G)
                    {
                        neighbor.G = costToNeighbour;
                        neighbor.Connection = currentCell;

                        if (!inSearch)
                        {
                            neighbor.H = neighbor.GetDistance(targetCell);
                            toSearch.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }
    }
}
