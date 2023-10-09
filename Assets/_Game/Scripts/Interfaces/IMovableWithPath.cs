using System.Collections.Generic;
using StrategyDemo.Logic;
using UnityEngine;

namespace StrategyDemo.Interfaces
{
    public interface IMovableWithPath
    {
        public void GoPath(Vector3[] path, List<CellInfo> targetCell, IDamageable damageable);
        public void SetIsSelectedObject(bool isActive);
    }
}