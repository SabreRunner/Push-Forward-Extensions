/*
	GenericLerper

	Description: Simplifies lerping values.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

using UnityEngine.Events;

namespace PushForward.Extenders
{
    using Base;
    using ExtensionMethods;
    using UnityEngine;

    public class GenericLerper : MonoBehaviour
    {
        [SerializeField] private float source;
        [SerializeField] private float timeInSeconds;
        [SerializeField] private FloatEvent lerpEvent;
        [SerializeField] private UnityEvent atEndEvent;

        public float Source { get => this.source; set => this.source = value; }

        public float TimeInSeconds { get => this.timeInSeconds; set => this.timeInSeconds = value; }

        public void LerpTo(float destination)
        {
            this.ActionEachFrameForSeconds(this.TimeInSeconds,
										   secondsPassed => this.lerpEvent?.Invoke(
												this.Source + (secondsPassed / this.TimeInSeconds).Clamp01() * (destination - this.Source)));
            this.ActionInSeconds(this.timeInSeconds, () => this.atEndEvent?.Invoke());
        }
    }
}
