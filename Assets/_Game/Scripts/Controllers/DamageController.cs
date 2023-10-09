using StrategyDemo.Interfaces;

namespace StrategyDemo.Controller
{
    public static class DamageController
    {
        public static void TakeDamage(IDamageable client, uint damage)
        {
            client.TakeDamage(damage);
        }
    }
}
