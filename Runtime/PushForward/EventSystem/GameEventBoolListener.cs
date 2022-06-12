
namespace PushForward.EventSystem
{
	using Base;
	using UnityEngine;

	public class GameEventBoolListener : GameEventListenerBase
    {
		/// <summary>This listener's event is an event with a number.</summary>
		[SerializeField] private GameEventBool gameEventBool;
		protected override GameEvent GameEvent => this.gameEventBool;
		/// <summary>This listener's event gets an integer.</summary>
		[SerializeField] private BoolEvent eventResponse;

		protected override void OnEventRaised()
		{ this.eventResponse?.Invoke(this.gameEventBool.condition); }
	}
}