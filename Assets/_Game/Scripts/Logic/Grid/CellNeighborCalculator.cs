using StrategyDemo.SO;
using StrategyDemo.Component;
using StrategyDemo.Interfaces;
using UnityEngine;

namespace StrategyDemo.Logic
{
    public static class CellNeighborCalculator
    {
        public static void GetNeighbor(ICellShape shape, GridGenerator generator, Vector2Int size)
        {
            shape.CalculateNeighbour(generator, size);
        }
    }
}