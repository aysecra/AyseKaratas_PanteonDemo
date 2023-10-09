using System.Collections.Generic;
using UnityEngine;

namespace StrategyDemo.SO
{
    [CreateAssetMenu(menuName = "SO/Building Unit")]

    public class BuildingUnitSO : UnitSO
    {
        [Header("UI Elements")]
        [SerializeField] private string name;
        [SerializeField] private Sprite image;
        [SerializeField] private string info;
        
        [Header("Placement Elements")]
        [SerializeField] private Vector2Int size;
        
        [Header("Damageable Elements")]
        [SerializeField] private uint health;
        
        [Header("Production Elements")]
        [SerializeField] private List<UnitSO> productUnitList;
        
        public override string Name => name;
        public override Sprite Image => image;
        public override string Info => info;
        public override Vector2Int Size => size;
        public override uint Health => health;
        public List<UnitSO> ProductUnitList => productUnitList;
    }
}
