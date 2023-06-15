/*
	GameEventListener

	Description: The receiver for GameEvents

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
	using UnityEngine;
	using UnityEngine.Events;

	/// <summary>The basic game event listener.
	///		Holds the basic empty game event.</summary>
	public class GameEventListener : GameEventListenerBase
    {
		/// <summary>The actual game event.</summary>
        [SerializeField] private GameEvent gameEvent;

		[SerializeField, Tooltip("The class to use to get the event.")] private GameEventGetter eventGetter;
		public override GameEvent GameEvent => this.gameEvent;
		/// <summary>The event response event is unique to every listener.</summary>
		[SerializeField] private UnityEvent eventResponse;

		/// <summary>Overriding for the basic listener is just a simple event.</summary>
        protected override void OnEventRaised()
        { this.eventResponse?.Invoke(); }
    }
}