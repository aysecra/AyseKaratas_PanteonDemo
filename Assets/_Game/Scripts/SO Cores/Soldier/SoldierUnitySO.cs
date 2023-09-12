using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo.SO
{
    [CreateAssetMenu(menuName = "SO/Soldier Unit")]
    public class SoldierUnitySO : ScriptableObject
    {
        [Header("UI Elements")] [SerializeField]
        private List<SoldierUnitSO> soldierList;

        public List<SoldierUnitSO> SoldierList => soldierList;
    }
}
