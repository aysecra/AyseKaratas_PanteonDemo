using System.Collections.Generic;
using PanteonDemo.Interfaces;
using PanteonDemo.Logic;
using UnityEngine;

namespace PanteonDemo.Controller
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
