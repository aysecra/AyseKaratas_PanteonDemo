using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public struct HittedObject<T> where T : class
    {
        public T HittedScript;
        public Transform HittedTransform;
    }

    public static class RaycastManager
    {
        public static bool SendRaycast(Vector3 startPoint, Vector3 direction, LayerMask targetLayer,
            out Transform hittedTransform, bool isDraw = false, Color drawColor = default)
        {
            drawColor = drawColor == default ? Color.red : drawColor;
            hittedTransform = null;
            RaycastHit hit;

            if (Physics.Raycast(startPoint, direction, out hit, Mathf.Infinity, targetLayer))
            {
                if (isDraw)
                    Debug.DrawRay(startPoint, direction * hit.distance, drawColor);
                hittedTransform = hit.transform;
                return true;
            }

            return false;
        }

        public static bool SendRaycast<T>(Vector3 startPoint, Vector3 direction, out T hittedScript,
            bool isDraw = false, Color drawColor = default) where T : class
        {
            drawColor = drawColor == default ? Color.red : drawColor;
            hittedScript = null;
            RaycastHit hit;

            if (Physics.Raycast(startPoint, direction, out hit, Mathf.Infinity))
            {
                if (hit.transform.TryGetComponent(out T hScript))
                {
                    if (isDraw)
                        Debug.DrawRay(startPoint, direction * hit.distance, drawColor);
                    hittedScript = hScript;
                    return true;
                }
            }

            return false;
        }

        public static bool SendRaycastAll(Vector3 startPoint, Vector3 direction, LayerMask targetLayer, bool
            isBreakFindObject, out Transform[] hittedTransforms, bool isDraw = false, Color drawColor = default)
        {
            drawColor = drawColor == default ? Color.red : drawColor;
            hittedTransforms = null;
            RaycastHit[] hits = Physics.RaycastAll(startPoint, direction, Mathf.Infinity, targetLayer);
            hittedTransforms = new Transform[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                hittedTransforms[i] = hits[i].transform;
                if (isDraw)
                    Debug.DrawRay(startPoint, direction * hits[i].distance, drawColor);
                if (isBreakFindObject)
                    return true;
            }

            if (hits.Length > 0)
                return true;

            return false;
        }

        public static bool SendRaycastAll<T>(Vector3 startPoint, Vector3 direction, bool isBreakFindObject,
            out List<T> hittedScrips, bool isDraw = false, Color drawColor = default) where T : class
        {
            drawColor = drawColor == default ? Color.red : drawColor;
            hittedScrips = null;
            RaycastHit[] hits = Physics.RaycastAll(startPoint, direction, Mathf.Infinity);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.TryGetComponent(out T hScript))
                {
                    if (isDraw)
                        Debug.DrawRay(startPoint, direction * hits[i].distance, drawColor);
                    hittedScrips.Add(hScript);
                    if (isBreakFindObject)
                        return true;
                }

                if (isDraw)
                    Debug.DrawRay(startPoint, direction * hits[i].distance, drawColor);
            }

            if (hittedScrips.Count > 0)
                return true;

            return false;
        }
    }
}