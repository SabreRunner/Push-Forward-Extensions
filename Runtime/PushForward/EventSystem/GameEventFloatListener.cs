
namespace PushForward.EventSystem
{
	using Base;
	using UnityEngine;

	public class GameEventFloatListener : GameEventListenerBase
    {
		/// <summary>This listener's event is an event with a number.</summary>
		[SerializeField] private GameEventFloat gameEventFloat;
		protected override GameEvent GameEvent => this.gameEventFloat;
		/// <summary>This listener's event gets an integer.</summary>
		[SerializeField] private FloatEvent eventResponse;

		protected override void OnEventRaised()
		{ this.eventResponse?.Invoke(this.gameEventFloat.@float); }
	}
}