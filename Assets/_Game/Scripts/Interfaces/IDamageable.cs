namespace StrategyDemo.Interfaces
{
    public interface IDamageable
    {
        public uint Health { get; }

        public void TakeDamage(uint damage);
        public void RestoreHealth();
        public void Die();
    }
}