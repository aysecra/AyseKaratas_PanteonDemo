using StrategyDemo.SO;
using UnityEngine;

namespace StrategyDemo.Interfaces
{
    public interface IChangeable
    {
        public UnitSO UnitySo { get; }
        public void SetType(UnitSO currUnit);
    }
}
