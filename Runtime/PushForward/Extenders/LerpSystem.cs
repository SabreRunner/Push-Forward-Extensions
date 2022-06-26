/*
 * Lerp System
 *
 * Description: Handles all kinds of lerps with convenience and options
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-26
*/

namespace PushForward.Extenders
{
    #region using
    using System;
    using System.Collections.Generic;
    using ExtensionMethods;
    using UnityEngine;
    using UnityEngine.Events;
    #endregion // using

    #region timed unity event
    /// <summary>A definition of an event to trigger during a lerp.</summary>
    [Serializable] public struct TimedUnityEvent
    {
        /// <summary>When during a lerp to trigger. Using progression not Animation Evaluated Progression.</summary>
        [Range(0f, 1f)] public float progressionTrigger;
        /// <summary>The event to trigger with the progression made.</summary>
        public UnityEvent<float> triggeredEvent;
    }
    #endregion // timed unity event

    /// <summary>Lerp Interface to allow system to handle all kinds of lerps.</summary>
    public interface ILerp
    {
        public bool Done { get; }
        public float Progression { get; }
        public void Execute(float progressionAddedInSeconds);
    }

    public class LerpSystem : SingletonBehaviour<MonoBehaviour>
    {
        /// <summary>The definition of a transform lerp.</summary>
        [Serializable] public class TransformLerp : ILerp
        {
            #region fields
            public readonly Transform transformToLerp;
            public readonly bool isRectTransform;
            public readonly Transform sourceTransform, targetTransform;
            public readonly float durationInSeconds;
            public readonly AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);
            public readonly bool reverse;
            public readonly TimedUnityEvent[] timedEvents;

            private float progressionTime;

            public float Progression => this.progressionTime;
            public bool Done => this.progressionTime >= this.durationInSeconds;

            public RectTransform RectTransformToLerp => (RectTransform)this.transformToLerp;
            public RectTransform SourceRectTransform => (RectTransform)this.sourceTransform;
            public RectTransform TargetRectTransform => (RectTransform)this.targetTransform;
            #endregion // fields

            #region events
            /// <summary>Calls for changes every fame while lerping.</summary>
            public event Action<Vector3> PositionUpdate;
            public event Action<Quaternion> RotationUpdate;
            public event Action<Vector3> ScaleUpdate;
            #endregion // events

            #region execution
            /// <summary>Lerp current selected lerp as a RectTransform.</summary>
            private void LerpRectSubroutine()
            {
                float fraction = (this.reverse ? this.durationInSeconds - this.progressionTime : this.progressionTime) / this.durationInSeconds;
                float evaluatedFraction = this.animationCurve.Evaluate(fraction);

                this.RectTransformToLerp.anchoredPosition = Vector3.Lerp(this.SourceRectTransform.anchoredPosition,
                                                                         this.TargetRectTransform.anchoredPosition, evaluatedFraction);
                this.RectTransformToLerp.pivot = Vector2.Lerp(this.SourceRectTransform.pivot, this.TargetRectTransform.pivot, evaluatedFraction);
                this.RectTransformToLerp.anchorMax = Vector2.Lerp(this.SourceRectTransform.anchorMax, this.TargetRectTransform.anchorMax, evaluatedFraction);
                this.RectTransformToLerp.anchorMin = Vector2.Lerp(this.SourceRectTransform.anchorMin, this.TargetRectTransform.anchorMin, evaluatedFraction);
                this.RectTransformToLerp.offsetMax = Vector2.Lerp(this.SourceRectTransform.offsetMax, this.TargetRectTransform.offsetMax, evaluatedFraction);
                this.RectTransformToLerp.sizeDelta = Vector2.Lerp(this.SourceRectTransform.sizeDelta, this.TargetRectTransform.sizeDelta, evaluatedFraction);
                this.RectTransformToLerp.localScale = Vector3.Lerp(this.sourceTransform.localScale, this.targetTransform.localScale, evaluatedFraction);
                this.RectTransformToLerp.eulerAngles = Vector3.Lerp(this.sourceTransform.eulerAngles, this.targetTransform.eulerAngles, evaluatedFraction);
            }

            /// <summary>Lerp the currently selected lerp as a regular Transform.</summary>
            private void LerpSubroutine()
            {
                float fraction = (this.reverse ? this.durationInSeconds - this.progressionTime : this.progressionTime) / this.durationInSeconds;
                float evaluatedFraction = this.animationCurve.Evaluate(fraction);

                Vector3 targetPosition = this.targetTransform.position;
                Vector3 sourcePosition = this.sourceTransform.position;
                this.transformToLerp.position = Vector3.Lerp(sourcePosition, targetPosition, evaluatedFraction);
                Quaternion sourceRotation = this.sourceTransform.rotation;
                Quaternion targetRotation = this.targetTransform.rotation;
                this.transformToLerp.rotation = Quaternion.Lerp(sourceRotation, targetRotation, evaluatedFraction);
                Vector3 targetScale = this.targetTransform.localScale;
                Vector3 sourceScale = this.sourceTransform.localScale;
                this.transformToLerp.localScale = Vector3.Lerp(sourceScale, targetScale, evaluatedFraction);

                this.PositionUpdate?.Invoke(targetPosition - sourcePosition);
                this.RotationUpdate?.Invoke(sourceRotation.RotationTo(targetRotation));
                this.ScaleUpdate?.Invoke(targetScale - sourceScale);
            }

            public void Execute(float progressionAddedInSeconds)
            {
                // update lerp time
                this.progressionTime += progressionAddedInSeconds;
                // lerp
                if (this.isRectTransform)
                { this.LerpRectSubroutine(); }
                else { this.LerpSubroutine(); }
                // activate events that happened in the progression
                foreach (TimedUnityEvent timedEvent in this.timedEvents)
                {
                    if (timedEvent.progressionTrigger.Between(this.progressionTime - progressionAddedInSeconds, this.progressionTime))
                    { timedEvent.triggeredEvent?.Invoke(this.progressionTime); }
                }
            }
            #endregion // execution

            #region creation
            public TransformLerp(Transform toLerp, Transform source, Transform target, float duration,
                                 AnimationCurve curve = null, TimedUnityEvent[] timedEvents = null, bool isRect = false, bool reverse = false)
            {
                this.transformToLerp = toLerp;
                this.isRectTransform = isRect;
                this.sourceTransform = source;
                this.targetTransform = target;
                this.durationInSeconds = duration;
                this.reverse = reverse;
                if (curve != null)
                { this.animationCurve = curve; }
                this.timedEvents = timedEvents;
            }
            public TransformLerp(Transform toLerp, Transform source, Transform target, float duration, TimedUnityEvent[] timedEvents)
                : this(toLerp, source, target, duration, null, timedEvents) { }
            #endregion // creation
        }

        #region fields
        private List<ILerp> activeLerps = null;
        #endregion // fields

        private void Update()
        {
            List<ILerp> doneLerps = new();

            foreach (ILerp activeLerp in this.activeLerps)
            {
                activeLerp.Execute(Time.deltaTime);
                if (activeLerp.Done)
                { doneLerps.Add(activeLerp); }
            }

            foreach (ILerp lerp in doneLerps)
            { this.activeLerps.Remove(lerp); }
        }

        private void Awake() => this.SetInstance(this);
    }
}
