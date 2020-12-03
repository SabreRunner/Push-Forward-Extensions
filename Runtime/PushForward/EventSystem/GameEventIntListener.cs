
using UnityEngine.Events;

namespace PushForward.EventSystem
{
	using Base;
	using UnityEngine;

	public class GameEventIntListener : GameEventListenerBase
    {
		/// <summary>This listener's event is an event with a number.</summary>
		[SerializeField] private GameEventInt gameEventInt;
		protected override GameEvent GameEvent => this.gameEventInt;
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
