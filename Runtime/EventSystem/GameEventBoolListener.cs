
namespace PushForward.EventSystem
{
	using Base;
	using UnityEngine;

	public class GameEventBoolListener : GameEventListenerBase
    {
		/// <summary>This listener's event is an event with a number.</summary>
		[SerializeField] private GameEventBool gameEventBool;
		// ReSharper disable once UnassignedField.Global -- assigned, if needed, at user runtime.
		public EventGetter<GameEventBool> boolEventGetter;
		public override GameEvent GameEvent => this.gameEventBool ??= this.boolEventGetter?.GetEventAction();
		/// <summary>This listener's event gets an integer.</summary>
		[SerializeField] private BoolEvent eventResponse;

		protected override void OnEventRaised()
		{ this.eventResponse?.Invoke(this.gameEventBool.condition); }
	}
}