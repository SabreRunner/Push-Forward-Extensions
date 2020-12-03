using System;
using UnityEngine;
using UnityEngine.Events;

namespace Soroka
{
    public class TransformLerper : BaseMonoBehaviour
    {
        [Serializable]
        public class TransformLerp
        {
            public Transform transform;
            public RectTransform sourceRectTransform, targetRectTransform;
            public float durationInSeconds;
            public AnimationCurve animationCurve;
            public UnityEvent onFinish;
        }

        [SerializeField] private TransformLerp[] transformLerps;
        private int lerpIndex;
        private TransformLerp IndexedLerp => this.transformLerps[this.lerpIndex];
        
        private void LerpRectSubroutine(float currentTime)
        {
            float fraction = currentTime / this.IndexedLerp.durationInSeconds;
            float evaluatedFraction = this.IndexedLerp.animationCurve.Evaluate(fraction);
            this.RectTransform.anchoredPosition = Vector3.Lerp(this.IndexedLerp.sourceRectTransform.anchoredPosition,
                this.IndexedLerp.targetRectTransform.anchoredPosition, evaluatedFraction);
            this.RectTransform.pivot = Vector2.Lerp(this.IndexedLerp.sourceRectTransform.pivot,
                this.IndexedLerp.targetRectTransform.pivot, evaluatedFraction);
            this.RectTransform.anchorMax = Vector2.Lerp(this.IndexedLerp.sourceRectTransform.anchorMax,
                this.IndexedLerp.targetRectTransform.anchorMax, evaluatedFraction);
            this.RectTransform.anchorMin = Vector2.Lerp(this.IndexedLerp.sourceRectTransform.anchorMin,
                this.IndexedLerp.targetRectTransform.anchorMin, evaluatedFraction);
            this.RectTransform.offsetMax = Vector2.Lerp(this.IndexedLerp.sourceRectTransform.offsetMax,
                this.IndexedLerp.targetRectTransform.offsetMax, evaluatedFraction);
            this.RectTransform.sizeDelta = Vector2.Lerp(this.IndexedLerp.sourceRectTransform.sizeDelta,
                this.IndexedLerp.targetRectTransform.sizeDelta, evaluatedFraction);
            this.RectTransform.localScale = Vector3.Lerp(this.IndexedLerp.sourceRectTransform.localScale,
                this.IndexedLerp.targetRectTransform.localScale, evaluatedFraction);
            this.RectTransform.eulerAngles = Vector3.Lerp(this.IndexedLerp.sourceRectTransform.eulerAngles,
                this.IndexedLerp.targetRectTransform.eulerAngles, evaluatedFraction);
        }
        
        public void LerpRectTransform(int lerpIndex)
        {
            this.lerpIndex = lerpIndex;
            this.ActionEachFrameForSeconds(this.LerpRectSubroutine, this.IndexedLerp.durationInSeconds);
            this.ActionInSeconds(()=>this.LerpRectSubroutine(this.IndexedLerp.durationInSeconds),
                                    this.IndexedLerp.durationInSeconds);
            this.ActionInSeconds(this.IndexedLerp.onFinish.Invoke, this.IndexedLerp.durationInSeconds);
        }
    }
}
