using UnityEngine;

namespace PanteonDemo.SO
{
    [CreateAssetMenu(menuName = "SO/Soldier Unit")]
    public class SoldierUnitSO : ScriptableObject
    {
        [Header("UI Elements")]
        [SerializeField] private string name;
        [SerializeField] private Sprite image;
        [SerializeField] private string info;
        
        [Header("Movement Elements")]
        [SerializeField] private float durationPerCell;
        
        [Header("Damageable Elements")]
        [SerializeField] private uint health;
        [SerializeField] private uint damage;

        public string Name => name;

        public Sprite Image => image;

        public string Info => info;

        public float DurationPerCell => durationPerCell;

        public uint Health => health;

        public uint Damage => damage;
    }
}