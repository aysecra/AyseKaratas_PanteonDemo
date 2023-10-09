using UnityEngine;

namespace StrategyDemo.SO
{
    [CreateAssetMenu(menuName = "SO/ChangeableComponent Unit")]
    public class SoldierUnitSO : UnitSO
    {
        [Header("UI Elements")]
        [SerializeField] private string name;
        [SerializeField] private Sprite image;
        [SerializeField] private string info;
        
        [Header("Movement Elements")]
        [SerializeField] private float durationPerCell;
        
        [Header("Placement Elements")]
        [SerializeField] private Vector2Int size;
        
        [Header("Damageable Elements")]
        [SerializeField] private uint health;
        [SerializeField] private uint damage;

        public override string Name => name;
        public override Sprite Image => image;
        public override string Info => info;
        public float DurationPerCell => durationPerCell;
        public override Vector2Int Size => size;
        public override uint Health => health;
        public uint Damage => damage;
    }
}