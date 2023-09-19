using UnityEngine;

namespace PanteonDemo.SO
{
    public abstract class UnitSO : ScriptableObject
    {
        public abstract string Name { get; }
        public abstract Sprite Image { get; }
        public abstract string Info { get; }
        public abstract Vector2Int Size { get; }
        public abstract uint Health { get; }

        public virtual bool IsEqual(UnitSO other)
        {
            return other == this;
        }
    }
}
