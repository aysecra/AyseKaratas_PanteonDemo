using System;
using DG.Tweening;
using PanteonDemo.Controller;
using PanteonDemo.Interfaces;
using PanteonDemo.SO;
using UnityEngine;

namespace PanteonDemo.Component
{
    public class SoldierMovement : MonoBehaviour
        , IMovableWithPath
    {
        private float _durationPerCell;

        public IPlaceable Placement
        {
            get { return Placement = Placement ?? GetComponent<IPlaceable>(); }
            set { }
        }
        
        public void SetSoldier(float duration)
        {
            _durationPerCell = duration;
        }

        public void GoPath(Vector3[] path)
        {
            transform.DOPath(path, _durationPerCell * path.Length)
                .OnStart((() => { PlayerController.Instance.IsClickable = false; }))
                .OnComplete((() => { PlacementController.Place(Placement); }));
        }
    }
}