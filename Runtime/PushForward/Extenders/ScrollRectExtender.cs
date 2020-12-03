
namespace PushForward.Extenders
{
    #region using
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using ExtensionMethods;
    using UnityEngine.Events;
    #endregion // using

    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectExtender : BaseMonoBehaviour
    {
        [Serializable]
        public class ScrollEvent
        {
            public enum CheckFor { X, Y, Both }

            #region fields
            public string elementName;
            public CheckFor checkFor;
            public Vector2 low;
            public Vector2 high;
            public bool previousInside;
            
            public UnityEvent actionOnEnter;
            public UnityEvent actionOnExit;
            #endregion // fields

            /// <summary>Check if the value is inside the specified area.</summary>
            /// <param name="scrollValue">The vector2 defining the position of the scrollrect</param>
            /// <returns>True if inside the area, false otherwise.</returns>
            private bool ValueInsideArea(Vector2 scrollValue)
            {
                switch (this.checkFor)
                {
                    case CheckFor.X: return scrollValue.x.Between(this.low.x, this.high.x);
                    case CheckFor.Y: return scrollValue.y.Between(this.low.y, this.high.y);
                    case CheckFor.Both: return scrollValue.Between(this.low, this.high);
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            
            /// <summary>Check a scroll value against the event and trigger accordingly.</summary>
            /// <param name="scrollValue">The value of the scroll rect to check</param>
            public void CheckEvent(Vector2 scrollValue)
            {
                bool newInside = this.ValueInsideArea(scrollValue);
                
                if (this.previousInside && !newInside)
                { this.actionOnExit?.Invoke(); }
                else if (!this.previousInside && newInside)
                { this.actionOnEnter?.Invoke(); }

                this.previousInside = newInside;
            }
        }
        
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private float lerpSpeed = 1f;
        [SerializeField] private ScrollEvent[] scrollEvents;

        public void LerpToHorizontalValue(float value)
        {
            float t = 0;
            float a = this.scrollRect.horizontalNormalizedPosition;

            this.ActionEachFrameWhilePredicate(
                () =>
                {
                    t += Time.deltaTime * this.lerpSpeed;
                    this.scrollRect.horizontalNormalizedPosition = Mathf.Lerp(a, value, t);
                }, () => !t.FloatEqual(1));
        }

        public void LerpToVerticalValue(float value)
        {
            float t = 0;
            float a = this.scrollRect.verticalNormalizedPosition;

            this.ActionEachFrameWhilePredicate(
                () =>
                {
                    t += Time.deltaTime * this.lerpSpeed;
                    this.scrollRect.verticalNormalizedPosition = Mathf.Lerp(a, value, t);
                }, () => !t.FloatEqual(1));
        }
        
        public void ValueChanged(Vector2 value)
        {
            foreach (ScrollEvent scrollEvent in this.scrollEvents)
            { scrollEvent.CheckEvent(value); }
        }

        private void OnEnable()
        {
            this.ValueChanged(this.scrollRect.normalizedPosition);
        }

        private void OnValidate()
        {
            if (this.scrollRect == null)
            { this.scrollRect = this.GetComponent<ScrollRect>(); }
        }
    }
}
