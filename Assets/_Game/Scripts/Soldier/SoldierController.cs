using DG.Tweening;
using UnityEngine;

namespace PanteonDemo
{
    [RequireComponent(typeof(PoolableObject))]
    public class SoldierController : MonoBehaviour
    {
        public void Move(Vector3[] path, float durationPerCell)
        {
            transform.DOPath(path, durationPerCell * path.Length);
        }
    }
}
