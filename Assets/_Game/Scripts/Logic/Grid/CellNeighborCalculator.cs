using PanteonDemo.Component;
using PanteonDemo.Interfaces;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Logic
{
    public static class CellNeighborCalculator
    {
        public static void GetNeighbor(ICellShape shape, GridGenerator generator, Vector2Int size)
        {
            shape.CalculateNeighbour(generator, size);
        }
    }
}