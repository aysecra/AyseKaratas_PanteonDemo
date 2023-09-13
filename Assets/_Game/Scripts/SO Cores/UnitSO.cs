using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo.SO
{
    public abstract class UnitSO : ScriptableObject
    {
        public abstract string Name { get; }
        public abstract Sprite Image { get; }
        public abstract string Info { get; }
        public abstract Vector2 Size { get; }
        public abstract uint Health { get; }
    }
}
