using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Interfaces
{
    public interface IChangeable
    {
        public UnitSO UnitySo { get; }
        public void SetType(UnitSO currUnit);
    }
}
