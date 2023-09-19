using System.Collections.Generic;
using PanteonDemo.Logic;
using UnityEngine;

namespace PanteonDemo.Interfaces
{
    public interface IMovableWithPath
    {
        public void GoPath(Vector3[] path, List<CellInfo> targetCell, IDamageable damageable);
        public void SetIsSelectedObject(bool isActive);
    }
}