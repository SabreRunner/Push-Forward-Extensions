/*
	GameEventIntListener

	Description: The receiver for GameEventint

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

using UnityEngine.Events;

namespace PushForward.EventSystem
{
	using Base;
	using UnityEngine;

	public class GameEventIntListener : GameEventListenerBase
    {
		/// <summary>This listener's event is an event with a number.</summary>
		[SerializeField] private GameEventInt gameEventInt;
		// ReSharper disable once UnassignedField.Global -- assigned, if needed, at user runtime.
		public EventGetter<GameEventInt> intEventGetter;
		public override GameEvent GameEvent => this.gameEventInt ??= this.intEventGetter?.GetEventAction();
		/// <summary>This listener's event gets an integer.</summary>
		public IntEvent intResponse;
		/// <summary>Activate event with int as index.</summary>
		public UnityEvent[] intAsIndexResponse;

		protected override void OnEventRaised()
		{
			this.intResponse?.Invoke(this.gameEventInt.integer);
			
			if (this.intAsIndexResponse != null
			    && this.gameEventInt.integer < this.intAsIndexResponse.Length
			    && this.gameEventInt.integer >= 0
			    && this.intAsIndexResponse[this.gameEventInt.integer] != null)
			{ this.intAsIndexResponse[this.gameEventInt.integer].Invoke(); }
		}
	}
}
