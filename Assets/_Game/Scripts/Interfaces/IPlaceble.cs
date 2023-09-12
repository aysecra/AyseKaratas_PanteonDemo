using UnityEngine;

namespace PanteonDemo.Interfaces
{
    public interface IPlaceble
    {
        public Vector2 Size { get; }
        public bool IsActivate { get; }

        public void Place();
    }
}
