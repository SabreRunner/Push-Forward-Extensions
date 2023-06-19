/*
	GameEventFloatListener

	Description: The receiver for GameEventFloat

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
	using Base;
	using UnityEngine;

	public class GameEventFloatListener : GameEventListenerBase
    {
		/// <summary>This listener's event is an event with a number.</summary>
		[SerializeField] private GameEventFloat gameEventFloat;
		// ReSharper disable once UnassignedField.Global -- assigned, if needed, at user runtime.
		public EventGetter<GameEventFloat> floatEventGetter;
		public override GameEvent GameEvent => this.gameEventFloat ??= this.floatEventGetter?.GetEventAction();
		/// <summary>This listener's event gets an integer.</summary>
		[SerializeField] private FloatEvent eventResponse;

		protected override void OnEventRaised()
		{ this.eventResponse?.Invoke(this.gameEventFloat.@float); }
	}
}