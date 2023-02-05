/*
 * ScrollRect Extender
 *
 * Description: Additional useful methods for ScrollRect (Lerping to value, triggering events 
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2023-2-5
*/

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
    public class ScrollRectExtender : MonoBehaviour
    {
        [Serializable]
        public class ScrollEvent
        {
            public enum CheckFor { Unknown, X, Y, Both }

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
            /// <param name="scrollValue">The vector2 defining the position of the scrollRect</param>
            /// <returns>True if inside the area, false otherwise.</returns>
            private bool ValueInsideArea(Vector2 scrollValue)
            {
                return this.checkFor switch
                    {
                        CheckFor.X => scrollValue.x.Between(this.low.x, this.high.x),
                        CheckFor.Y => scrollValue.y.Between(this.low.y, this.high.y),
                        CheckFor.Both => scrollValue.Between(this.low, this.high),
                        CheckFor.Unknown => throw new ArgumentOutOfRangeException(),
                        _ => throw new ArgumentOutOfRangeException()
                    };
            }

            /// <summary>Check a scroll value against the event and trigger accordingly.</summary>
            /// <param name="scrollValue">The value of the scroll rect to check</param>
            public void CheckEvent(Vector2 scrollValue)
            {
                bool newInside = this.ValueInsideArea(scrollValue);

                switch (this.previousInside)
                {
                    case true when !newInside: this.actionOnExit?.Invoke(); break;
                    case false when newInside: this.actionOnEnter?.Invoke(); break;
                }

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

            this.ActionEachFrameWhilePredicate(() => !t.IsApproximately(1),
                                               () =>
                                               {
                                                   t += Time.deltaTime * this.lerpSpeed;
                                                   this.scrollRect.horizontalNormalizedPosition = Mathf.Lerp(a, value, t);
                                               });
        }

        public void LerpToVerticalValue(float value)
        {
            float t = 0;
            float a = this.scrollRect.verticalNormalizedPosition;

            this.ActionEachFrameWhilePredicate(()=> !t.IsApproximately(1),
                () =>
                {
                    t += Time.deltaTime * this.lerpSpeed;
                    this.scrollRect.verticalNormalizedPosition = Mathf.Lerp(a, value, t);
                });
        }

        public void ValueChanged(Vector2 value)
        {
            foreach (ScrollEvent scrollEvent in this.scrollEvents)
            { scrollEvent.CheckEvent(value); }
        }

        private void OnEnable() => this.ValueChanged(this.scrollRect.normalizedPosition);

        private void OnValidate()
        {
            if (this.scrollRect == null)
            { this.scrollRect = this.GetComponent<ScrollRect>(); }
        }
    }
}
