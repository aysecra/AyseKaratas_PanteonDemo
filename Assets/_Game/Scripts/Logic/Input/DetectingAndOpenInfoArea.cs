using StrategyDemo.Controller;
using StrategyDemo.Interfaces;
using StrategyDemo.Manager;
using UnityEngine;

namespace StrategyDemo.Logic
{
    public static class DetectingAndOpenInfoArea
    {
        public static void OpenInfo(Camera mainCamera)
        {
            Vector2 currentMousePoint = InputManager.Instance.CurrentPosition;
            Vector3 origin = mainCamera.ScreenToWorldPoint(currentMousePoint);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.TryGetComponent(out IClickable clickable))
                {
                    ClickController.OnClick(clickable);
                    break;
                }
            }
        }
    }
}