using UnityEngine;

namespace PanteonDemo.Interfaces
{
    public interface IMovableWithPath
    {
        public void GoPath(Vector3[] path);
    }
}