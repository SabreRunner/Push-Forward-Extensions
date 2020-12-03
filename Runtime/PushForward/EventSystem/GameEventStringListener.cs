namespace PushForward.EventSystem
{
    using Base;
    using UnityEngine;

    public class GameEventStringListener : GameEventListenerBase
    {
        /// <summary>This listener's event is an event with a number.</summary>
        [SerializeField] private GameEventString gameEventString;
        protected override GameEvent GameEvent => this.gameEventString;
        /// <summary>This listener's event gets an integer.</summary>
        [SerializeField] private StringEvent eventResponse;

        protected override void OnEventRaised()
        { this.eventResponse?.Invoke(this.gameEventString.@string); }
    }
}