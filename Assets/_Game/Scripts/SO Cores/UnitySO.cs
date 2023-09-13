using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo.SO
{
    [CreateAssetMenu(menuName = "SO/Unity of Placeable Units")]
    public class UnitySO : ScriptableObject
    {
        [Header("UI Elements")] [SerializeField]
        private List<UnitSO> unitList;

        public List<UnitSO> UnitList => unitList;
    }
}
