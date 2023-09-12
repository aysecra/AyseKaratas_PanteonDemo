
namespace PanteonDemo.Interfaces
{
    public interface IDamageable
    {
        public uint Health { get; }
        public uint Damage { get; }

        public void TakeDamage(int damage);
        public void RestoreHealth();
        public void Die();
    }
}