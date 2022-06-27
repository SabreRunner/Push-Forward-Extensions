/*
 * Lerp System
 *
 * Description: Handles all kinds of lerps with convenience and options
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-27
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
        /// <summary>When during a lerp to trigger. Using progression [0,1] not Animation Evaluated Progression.</summary>
        [Range(0f, 1f)] public float progressionTrigger;
        /// <summary>The event to trigger with the progression made.</summary>
        public UnityEvent<float> triggeredEvent;
    }
    #endregion // timed unity event

    /// <summary>Lerp Interface to allow system to handle all kinds of lerps.</summary>
    public interface ILerp
    {
        /// <summary>Whether this lerp has done its thing.</summary>
        public bool IsDone { get; }
        /// <summary>The unevaluated progression of the lerp.</summary>
        public float Progression { get; }
        /// <summary>The events to trigger during the lerp.</summary>
        public TimedUnityEvent[] TimedEvents { get; }
        /// <summary>Execute progression on this lerp.</summary>
        /// <param name="timeSinceLastFrame">The time difference between this frame and the last.</param>
        public void Execute(float timeSinceLastFrame);
    }

    [Serializable]
    public class BaseLerp
    {
        #region fields
        [SerializeField] protected float durationInSeconds;
        [SerializeField] protected AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] protected bool reverse;
        [SerializeField] protected TimedUnityEvent[] timedEvents;

        protected float progressionTime;
        #endregion // fields

        public float Progression => this.progressionTime;
        public bool IsDone => this.Progression >= this.durationInSeconds;
        public TimedUnityEvent[] TimedEvents => this.timedEvents;
    }

    /// <summary>The definition of a transform lerp.</summary>
    [Serializable] public class TransformLerp : BaseLerp, ILerp
    {
        #region fields
        [SerializeField] private Transform transformToLerp;
        [SerializeField] private bool isRectTransform;
        [SerializeField] private Transform sourceTransform, targetTransform;

        public RectTransform RectTransformToLerp => (RectTransform)this.transformToLerp;
        public RectTransform SourceRectTransform => (RectTransform)this.sourceTransform;
        public RectTransform TargetRectTransform => (RectTransform)this.targetTransform;
        #endregion // fields

        #region events
        /// <summary>Calls for changes every fame while lerping.</summary>
        public event Action<Vector3> PositionFromSourceUpdate;
        public event Action<Quaternion> RotationFromSourceUpdate;
        public event Action<Vector3> ScaleFromSourceUpdate;
        public event Action<Vector3> AnchoredPositionFromSourceUpdate;
        public event Action<Vector2> PivotFromSourceUpdate;
        public event Action<Vector2> AnchorMaxFromSourceUpdate;
        public event Action<Vector2> AnchorMinFromSourceUpdate;
        public event Action<Vector2> OffsetMaxFromSourceUpdate;
        public event Action<Vector2> OffsetMinFromSourceUpdate;
        public event Action<Vector2> SizeDeltaFromSourceUpdate;
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
            this.RectTransformToLerp.anchorMax
                = Vector2.Lerp(this.SourceRectTransform.anchorMax, this.TargetRectTransform.anchorMax, evaluatedFraction);
            this.RectTransformToLerp.anchorMin
                = Vector2.Lerp(this.SourceRectTransform.anchorMin, this.TargetRectTransform.anchorMin, evaluatedFraction);
            this.RectTransformToLerp.offsetMax
                = Vector2.Lerp(this.SourceRectTransform.offsetMax, this.TargetRectTransform.offsetMax, evaluatedFraction);
            this.RectTransformToLerp.offsetMin
                = Vector2.Lerp(this.SourceRectTransform.offsetMin, this.TargetRectTransform.offsetMin, evaluatedFraction);
            this.RectTransformToLerp.sizeDelta
                = Vector2.Lerp(this.SourceRectTransform.sizeDelta, this.TargetRectTransform.sizeDelta, evaluatedFraction);
            this.RectTransformToLerp.localScale = Vector3.Lerp(this.sourceTransform.localScale, this.targetTransform.localScale, evaluatedFraction);
            this.RectTransformToLerp.eulerAngles
                = Vector3.Lerp(this.sourceTransform.eulerAngles, this.targetTransform.eulerAngles, evaluatedFraction);

            this.AnchoredPositionFromSourceUpdate?.Invoke(this.RectTransformToLerp.anchoredPosition - this.SourceRectTransform.anchoredPosition);
            this.PivotFromSourceUpdate?.Invoke(this.RectTransformToLerp.pivot - this.SourceRectTransform.pivot);
            this.AnchorMaxFromSourceUpdate?.Invoke(this.RectTransformToLerp.anchorMax - this.SourceRectTransform.anchorMax);
            this.AnchorMinFromSourceUpdate?.Invoke(this.RectTransformToLerp.anchorMin - this.SourceRectTransform.anchorMin);
            this.OffsetMaxFromSourceUpdate?.Invoke(this.RectTransformToLerp.offsetMax - this.SourceRectTransform.offsetMax);
            this.OffsetMinFromSourceUpdate?.Invoke(this.RectTransformToLerp.offsetMin - this.SourceRectTransform.offsetMin);
            this.SizeDeltaFromSourceUpdate?.Invoke(this.RectTransformToLerp.sizeDelta - this.SourceRectTransform.sizeDelta);

            this.RotationFromSourceUpdate?.Invoke(this.RectTransformToLerp.rotation.RotationTo(this.RectTransformToLerp.rotation));
            this.ScaleFromSourceUpdate?.Invoke(this.RectTransformToLerp.localScale - this.SourceRectTransform.localScale);
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

            this.PositionFromSourceUpdate?.Invoke(this.transformToLerp.position - sourcePosition);
            this.RotationFromSourceUpdate?.Invoke(sourceRotation.RotationTo(this.transformToLerp.rotation));
            this.ScaleFromSourceUpdate?.Invoke(this.transformToLerp.localScale - sourceScale);
        }

        public void Execute(float timeSinceLastFrame)
        {
            // update lerp time
            this.progressionTime += timeSinceLastFrame;

            // lerp
            if (this.isRectTransform)
            { this.LerpRectSubroutine(); }
            else { this.LerpSubroutine(); }

            // activate events that happened in the progression
            foreach (TimedUnityEvent timedEvent in this.timedEvents)
            {
                if (timedEvent.progressionTrigger.Between(this.progressionTime - timeSinceLastFrame, this.progressionTime))
                { timedEvent.triggeredEvent?.Invoke(this.progressionTime); }
            }
        }
        #endregion // execution

        #region creation
        public TransformLerp(Transform toLerp, Transform source, Transform target, float duration, AnimationCurve curve = null,
                             TimedUnityEvent[] timedEvents = null, bool isRect = false, bool reverse = false)
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

    public class LerpSystem : SingletonBehaviour<MonoBehaviour>
    {
        #region fields
        private List<ILerp> activeLerps = null;
        #endregion // fields

        private void Update()
        {
            // save done lerps as to not modify a list mid iteration
            List<ILerp> doneLerps = new();

            foreach (ILerp activeLerp in this.activeLerps)
            {
                // execute the lerp
                activeLerp.Execute(Time.deltaTime);
                // if it's done, save for removal
                if (activeLerp.IsDone)
                { doneLerps.Add(activeLerp); }
            }

            // remove done lerps
            foreach (ILerp lerp in doneLerps)
            { this.activeLerps.Remove(lerp); }
        }

        private void Awake() => this.SetInstance(this);
    }
}
