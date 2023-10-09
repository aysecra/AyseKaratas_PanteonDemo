using StrategyDemo.Component;
using UnityEngine;

namespace StrategyDemo.Interfaces
{
    public interface ICellShape
    {
        public void CalculateNeighbour(GridGenerator generator, Vector2Int size);
    }

}