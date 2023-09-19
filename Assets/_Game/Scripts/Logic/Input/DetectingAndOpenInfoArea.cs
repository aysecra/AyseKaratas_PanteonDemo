using PanteonDemo.Controller;
using PanteonDemo.Interfaces;
using PanteonDemo.Manager;
using UnityEngine;

namespace PanteonDemo.Logic
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