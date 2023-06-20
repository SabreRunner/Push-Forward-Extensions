/*
	GameEventStringListener

	Description: The receiver for GameEventStrings.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
    using Base;
    using UnityEngine;

    public class GameEventStringListener : GameEventListenerBase
    {
        /// <summary>This listener's event is an event with a number.</summary>
        [SerializeField] private GameEventString gameEventString;
		// ReSharper disable once UnassignedField.Global -- assigned, if needed, at user runtime.
		private EventGetter<GameEventString> stringEventGetter;
		public override GameEvent GameEvent => this.gameEventString ??= this.stringEventGetter?.GetEventAction();
		public EventGetter<GameEventString> StringEventGetter
		{
			get => this.stringEventGetter;
			set
			{
				Object.Destroy(this.gameEventString);
				this.gameEventString = null;
				this.stringEventGetter = value;
			}
		}

        /// <summary>This listener's event gets an integer.</summary>
        [SerializeField] private StringEvent eventResponse;

        protected override void OnEventRaised()
        { this.eventResponse?.Invoke(this.gameEventString.@string); }
    }
}