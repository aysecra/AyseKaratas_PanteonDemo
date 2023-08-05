using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public static class Raycast2DManager
    {
        public static bool SendRaycast(Vector2 startPoint, Vector2 direction, LayerMask targetLayer,
            out Transform hittedTransform, bool isDraw = false, Color drawColor = default)
        {
            drawColor = drawColor == default ? Color.red : drawColor;
            hittedTransform = null;
            RaycastHit2D hit = Physics2D.Raycast(startPoint, direction, Mathf.Infinity, targetLayer);

            if (hit.collider != null)
            {
                // if (isDraw)
                // Debug.DrawRay(startPoint, direction * hit.distance, drawColor);
                hittedTransform = hit.transform;
                return true;
            }

            return false;
        }

        public static bool SendRaycast<T>(Vector2 startPoint, Vector2 direction, out T hittedScript,
            bool isDraw = false, Color drawColor = default) where T : class
        {
            drawColor = drawColor == default ? Color.red : drawColor;
            hittedScript = null;
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, direction, Mathf.Infinity);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.TryGetComponent(out T hScript))
                {
                    if (isDraw)
                        Debug.DrawRay(startPoint, direction * hits[i].distance, drawColor);
                    hittedScript = hScript;
                    return true;
                }

                if (isDraw)
                    Debug.DrawRay(startPoint, direction * hits[i].distance, drawColor);
            }

            return false;
        }

        public static bool SendRaycastAll(Vector2 startPoint, Vector2 direction, LayerMask targetLayer,
            out Transform[] hittedTransforms, bool isDraw = false, Color drawColor = default)
        {
            drawColor = drawColor == default ? Color.red : drawColor;
            hittedTransforms = null;
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, direction, Mathf.Infinity, targetLayer);
            hittedTransforms = new Transform[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                hittedTransforms[i] = hits[i].transform;
                if (isDraw)
                    Debug.DrawRay(startPoint, direction * hits[i].distance, drawColor);
            }

            if (hits.Length > 0)
                return true;

            return false;
        }

        public static bool SendRaycastAll<T>(Vector2 startPoint, Vector2 direction,
            out List<T> hittedScripts, bool isDraw = false, Color drawColor = default) where T : class
        {
            drawColor = drawColor == default ? Color.red : drawColor;
            hittedScripts = null;
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, direction, Mathf.Infinity);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.TryGetComponent(out T hScript))
                {
                    if (isDraw)
                        Debug.DrawRay(startPoint, direction * hits[i].distance, drawColor);
                    hittedScripts.Add(hScript);
                }

                if (isDraw)
                    Debug.DrawRay(startPoint, direction * hits[i].distance, drawColor);
            }

            if (hittedScripts.Count > 0)
                return true;

            return false;
        }
        
        public static bool DetectTouchedObject(Vector2 touchPosition, out Transform hittedTransform, int layerMask)
        {
            Vector2 origin = Camera.main.ScreenToWorldPoint(touchPosition);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, layerMask);

            hittedTransform = null;
            
            if (hit.collider != null)
            {
                hittedTransform = hit.transform;
                return true;
            }

            return false;
        }

        public static bool DetectTouchedObject<T>(Vector2 touchPosition, out T hittedScript)
            where T : class
        {
            Vector2 origin = Camera.main.ScreenToWorldPoint(touchPosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero);
            hittedScript = null;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.TryGetComponent(out T hitScript))
                {
                    hittedScript = hitScript;
                    return true;
                }
            }

            return false;
        }

        public static bool DetectTouchedObjects(Vector2 touchPosition, out Transform[] hittedTransform, int layerMask)
        {
            Vector2 origin = Camera.main.ScreenToWorldPoint(touchPosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero, layerMask);
            hittedTransform = new Transform[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                hittedTransform[i] = hits[i].transform;
            }

            if (hittedTransform.Length > 0)
                return true;

            return false;
        }

        public static bool DetectTouchedObjects<T>(Vector2 touchPosition, out List<T> hittedScripts)
            where T : class
        {
            Vector2 origin = Camera.main.ScreenToWorldPoint(touchPosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero);
            hittedScripts = new List<T>();

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.TryGetComponent(out T hitScript))
                {
                    hittedScripts.Add(hitScript);
                }
            }

            if (hittedScripts.Count > 0)
                return true;

            return false;
        }
    }
}