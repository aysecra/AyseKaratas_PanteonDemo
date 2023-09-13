using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo.Interfaces
{
    public interface IHitable
    {
        public uint Damage { get; }

        public void Hit(IDamageable target);
    }
}
