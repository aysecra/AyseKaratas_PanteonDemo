using DG.Tweening;
using PanteonDemo.Interfaces;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Object
{
    public class SoldierMovement : MonoBehaviour
                                    ,IMovableWithPath
                                    ,IPlaceble
    {
        public SoldierUnitSO _soldierUnitSo;
        public Vector2 Size { get; }
        public bool IsActivate { get; }

        public void GoPath(Vector3[] path)
        {
            transform.DOPath(path, _soldierUnitSo.DurationPerCell * path.Length)
                .OnStart((() => { PlayerController.Instance.IsClickable = false; }))
                .OnComplete((() => { Place(); }));
        }


        public void Place()
        {
            throw new System.NotImplementedException();
        }
    }
}