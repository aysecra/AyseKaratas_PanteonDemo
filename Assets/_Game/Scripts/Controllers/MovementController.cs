using System.Collections.Generic;
using StrategyDemo.Interfaces;
using StrategyDemo.Logic;
using UnityEngine;

namespace StrategyDemo.Controller
{
    public static class MovementController
    {
        public static void MoveWithPath(IMovableWithPath client, Vector3[] path, CellInfo targetCell, IDamageable damageable = null)
        {
            List<CellInfo> targetCellList = new List<CellInfo>();
            targetCellList.Add(targetCell);
           client.GoPath(path, targetCellList, damageable); 
        }
    }
}
