/*
 * TransformLerper
 *
 * Description: Allows describing various animations in order to lerp from one transform to another
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-13
*/

namespace PushForward.Extenders
{
    using System;
    using System.Linq;
    using ExtensionMethods;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>Lerp between two transforms.</summary>
    public class TransformLerper : MonoBehaviour
    {
        /// <summary>The definition of a transform lerp.</summary>
        [Serializable]
        public class TransformLerp
        {
            public Transform transformToLerp;
            public bool isRectTransform;
            public Transform sourceTransform, targetTransform;
            public float durationInSeconds;
            public AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);
            public bool reverse;
            public UnityEvent onFinish;

            public RectTransform SourceRectTransform => (RectTransform)this.sourceTransform;
            public RectTransform TargetRectTransform => (RectTransform)this.targetTransform;

            public static TransformLerp Create(Transform toLerp, bool isRect, Transform source, Transform target, float duration, bool reverse)
                => new TransformLerp {
                                        transformToLerp = toLerp, isRectTransform = isRect, sourceTransform = source,
                                        targetTransform = target, durationInSeconds = duration, reverse = reverse,
                                        animationCurve = AnimationCurve.Linear(0,0,1,1)
                                     };

            public static TransformLerp Create(Transform toLerp, bool isRect, Transform source, Transform target,
                                               float duration, bool reverse, AnimationCurve curve)
            {
                TransformLerp transformLerp = TransformLerp.Create(toLerp, isRect, source, target, duration, reverse);
                transformLerp.animationCurve = curve;
                return transformLerp;
            }

            public static TransformLerp Create() => TransformLerp.Create(null, false, null, null, 0, false,
                                                                         AnimationCurve.Linear(0,0,1,1));
        }

        #region fields
        [Tooltip("The array of lerps to choose from")]
        [SerializeField] private TransformLerp[] transformLerps;

        /// <summary>Calls for changes every fame while lerping.</summary>
        public event Action<Vector3> PositionUpdate;
        public event Action<Quaternion> RotationUpdate;
        public event Action<Vector3> ScaleUpdate;

        /// <summary>The lerp being lerped currently.</summary>
        private int currentLerpIndex;
        /// <summary>Get the current lerp data.</summary>
        private TransformLerp CurrentLerp => this.transformLerps[this.currentLerpIndex];
        #endregion // fields

        /// <summary>Toggles a given lerp's reverse.</summary>
        /// <remarks>Whether the lerp is done source to target (regular) or target to source (reversed) </remarks>
        /// <param name="index">The lerp to reverse.</param>
        public void ReverseLerp(int index) => this.transformLerps[index].reverse = !this.transformLerps[index].reverse;

        /// <summary>Overrides values for a given lerp.</summary>
        /// <remarks>Any value can be avoided except index. An avoided value will not be changed. IsRect will be set to false.</remarks>
        /// <param name="index">The lerp index to change.</param>
        /// <param name="toLerp">New transform to lerp.</param>
        /// <param name="isRect">Is the transform a RectTransform</param>
        /// <param name="source">New source transform.</param>
        /// <param name="target">New target transform.</param>
        /// <param name="durationInSeconds">New duration in seconds.</param>
        public void OverrideLerp(int index, Transform target = null, Transform source = null, float durationInSeconds = -1,
                                 Transform toLerp = null, bool isRect = false)
        {
            if (index < this.transformLerps.Length)
            {
                if (toLerp != null) { this.transformLerps[index].transformToLerp = toLerp; }
                if (source != null) { this.transformLerps[index].sourceTransform = source; }
                if (target != null) { this.transformLerps[index].targetTransform = target; }
                if (durationInSeconds >= 0) { this.transformLerps[index].durationInSeconds = durationInSeconds; }
                this.transformLerps[index].isRectTransform = isRect;
                this.transformLerps[index].reverse = false;
            }
            else
            { this.transformLerps = this.transformLerps.ToList().ChainAdd(
                                        TransformLerp.Create(toLerp, isRect, source, target, durationInSeconds, false)).ToArray(); }
        }

        /// <summary>Lerp current selected lerp as a RectTransform.</summary>
        /// <param name="currentTime">The time inside the lerp.</param>
        private void LerpRectSubroutine(float currentTime)
        {
            float fraction = (this.CurrentLerp.reverse ? this.CurrentLerp.durationInSeconds - currentTime : currentTime)
                                / this.CurrentLerp.durationInSeconds;
            float evaluatedFraction = this.CurrentLerp.animationCurve.Evaluate(fraction);

            this.CurrentLerp.transformToLerp.GetRectTransform().anchoredPosition = Vector3.Lerp(this.CurrentLerp.SourceRectTransform.anchoredPosition,
                                                                                                this.CurrentLerp.TargetRectTransform.anchoredPosition, evaluatedFraction);
            this.CurrentLerp.transformToLerp.GetRectTransform().pivot = Vector2.Lerp(this.CurrentLerp.SourceRectTransform.pivot,
                                                                                     this.CurrentLerp.TargetRectTransform.pivot, evaluatedFraction);
            this.CurrentLerp.transformToLerp.GetRectTransform().anchorMax = Vector2.Lerp(this.CurrentLerp.SourceRectTransform.anchorMax,
                                                                                         this.CurrentLerp.TargetRectTransform.anchorMax, evaluatedFraction);
            this.CurrentLerp.transformToLerp.GetRectTransform().anchorMin = Vector2.Lerp(this.CurrentLerp.SourceRectTransform.anchorMin,
                                                                                         this.CurrentLerp.TargetRectTransform.anchorMin, evaluatedFraction);
            this.CurrentLerp.transformToLerp.GetRectTransform().offsetMax = Vector2.Lerp(this.CurrentLerp.SourceRectTransform.offsetMax,
                                                                                         this.CurrentLerp.TargetRectTransform.offsetMax, evaluatedFraction);
            this.CurrentLerp.transformToLerp.GetRectTransform().sizeDelta = Vector2.Lerp(this.CurrentLerp.SourceRectTransform.sizeDelta,
                                                                                         this.CurrentLerp.TargetRectTransform.sizeDelta, evaluatedFraction);
            this.CurrentLerp.transformToLerp.GetRectTransform().localScale = Vector3.Lerp(this.CurrentLerp.sourceTransform.localScale,
                                                                                          this.CurrentLerp.targetTransform.localScale, evaluatedFraction);
            this.CurrentLerp.transformToLerp.GetRectTransform().eulerAngles = Vector3.Lerp(this.CurrentLerp.sourceTransform.eulerAngles,
                                                                                           this.CurrentLerp.targetTransform.eulerAngles, evaluatedFraction);
        }

        /// <summary>Lerp the currently selected lerp as a regular Transform.</summary>
        /// <param name="currentTime">The time inside the lerp.</param>
        private void LerpSubroutine(float currentTime)
        {
            float fraction = (this.CurrentLerp.reverse ? this.CurrentLerp.durationInSeconds - currentTime : currentTime)
                                / this.CurrentLerp.durationInSeconds;
            float evaluatedFraction = this.CurrentLerp.animationCurve.Evaluate(fraction);

            Vector3 targetPosition = this.CurrentLerp.targetTransform.position;
            Vector3 sourcePosition = this.CurrentLerp.sourceTransform.position;
            this.CurrentLerp.transformToLerp.position = Vector3.Lerp(sourcePosition, targetPosition, evaluatedFraction);

            // this.CurrentLerp.transformToLerp.eulerAngles =
            // Vector3.Lerp(this.CurrentLerp.sourceTransform.eulerAngles, this.CurrentLerp.targetTransform.eulerAngles, evaluatedFraction);
            Quaternion sourceRotation = this.CurrentLerp.sourceTransform.rotation;
            Quaternion targetRotation = this.CurrentLerp.targetTransform.rotation;
            this.CurrentLerp.transformToLerp.rotation = Quaternion.Lerp(sourceRotation, targetRotation, evaluatedFraction);

            Vector3 targetScale = this.CurrentLerp.targetTransform.localScale;
            Vector3 sourceScale = this.CurrentLerp.sourceTransform.localScale;
            this.CurrentLerp.transformToLerp.localScale = Vector3.Lerp(sourceScale, targetScale, evaluatedFraction);

            this.PositionUpdate?.Invoke(targetPosition - sourcePosition);
            this.RotationUpdate?.Invoke(sourceRotation.RotationTo(targetRotation));
            this.ScaleUpdate?.Invoke(targetScale - sourceScale);

            // this.Temp("Position: " + this.CurrentLerp.transformToLerp.position.StringRepresentation()
            // + "; Rotation: " + this.CurrentLerp.transformToLerp.rotation.eulerAngles.StringRepresentation());
        }

        /// <summary>Lerp using the data in the given transform lerp.</summary>
        /// <param name="newLerpIndex">The index of the lerp to use.</param>
        public void LerpTransform(int newLerpIndex)
        {
            // Copy a transform, hopefully
            this.currentLerpIndex = newLerpIndex;

            // this.Temp("Lerp Duration: " + this.CurrentLerp.durationInSeconds
                      // + "; Lerp Source Rotation: " + this.CurrentLerp.sourceTransform.eulerAngles.StringRepresentation()
                      // + "; Lerp Target Rotation: " + this.CurrentLerp.targetTransform.eulerAngles.StringRepresentation());
            // lerp according to the transform type
            if (this.CurrentLerp.isRectTransform)
            { this.ActionEachFrameForSeconds(this.LerpRectSubroutine, this.CurrentLerp.durationInSeconds); }
            else { this.ActionEachFrameForSeconds(this.LerpSubroutine, this.CurrentLerp.durationInSeconds); }
            this.ActionInSeconds(this.CurrentLerp.onFinish.Invoke, this.CurrentLerp.durationInSeconds);
        }

        private void OnValidate()
        {
            if (this.transformLerps == null || this.transformLerps.Length == 0)
            { this.transformLerps = new TransformLerp[1]; }

            for (int lerpIndex = 0; lerpIndex < this.transformLerps.Length; lerpIndex++)
            {
                if (this.transformLerps[lerpIndex] == null || this.transformLerps[lerpIndex].transformToLerp == null)
                { this.transformLerps[lerpIndex] = TransformLerp.Create(); }
            }
        }
    }
}
