using PanteonDemo.Component;
using UnityEngine;

namespace PanteonDemo.Interfaces
{
    public interface ICellShape
    {
        public void CalculateNeighbour(GridGenerator generator, Vector2Int size);
    }

}