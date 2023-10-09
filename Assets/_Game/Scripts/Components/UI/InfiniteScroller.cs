using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace StrategyDemo.Component
{
    public abstract class InfiniteScroller : ObjectPool
    {
        [SerializeField] protected ScrollRect scrollRect;
        [SerializeField] protected Transform scrollViewTransform;
        [SerializeField] protected float outOfBoundsThreshold = 50f;
        [SerializeField] protected Vector2 elementSize = new Vector2(55, 55);
        [SerializeField] protected float elementSpace = 10f;

        protected int _childCount;
        protected float _height;

        protected Vector2 _lastPos;
        protected bool _isPositiveDirection;
        protected bool _isStart;


        IEnumerator Start()
        {
            base.Start();

            scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            _height = Screen.height;
            scrollRect.content.localPosition = Vector3.up * _height * 2f;

            yield return new WaitForSeconds(1f);
            int counter = 0;
            while (counter < 300)
            {
                // _isPositiveDirection = true;
                OnValueChanged(Vector2.zero);
                counter++;
                scrollRect.content.transform.Translate(Time.deltaTime * 2f * Vector2.up);
                yield return new WaitForSeconds(.05f);
            }
        }


        protected abstract void OnValueChanged(Vector2 other);

        protected bool IsReachedThreshold(Transform element)
        {
            var position = scrollViewTransform.position;
            float verticalUpTreshold = position.y + outOfBoundsThreshold + _height * .5f;
            float verticalDownTreshold = position.y - outOfBoundsThreshold - _height * .5f;

            return _isPositiveDirection
                ? element.position.y - elementSize.y * .5f > verticalUpTreshold
                : element.position.y + elementSize.y * .5f < verticalDownTreshold;
        }

        private void OnEnable()
        {
            scrollRect.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            scrollRect.onValueChanged.RemoveListener(OnValueChanged);
        }
    }
}