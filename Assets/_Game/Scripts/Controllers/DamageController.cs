using System.Collections;
using System.Collections.Generic;
using PanteonDemo.Interfaces;
using UnityEngine;

namespace PanteonDemo.Controller
{
    public static class DamageController
    {
        public static void TakeDamage(IDamageable client, int damage)
        {
            client.TakeDamage(damage);
        }
    }
}
