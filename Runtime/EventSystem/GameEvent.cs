/*
	GameEvent

	Description: The building block of the Pub/Sub framework.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>The very basic game event.
	///		Just registers listeners and raises events.</summary>
    [CreateAssetMenu(menuName = "Event System/Game Event", order = 20)]
    public class GameEvent : ScriptableObject
    {
		[SerializeField] private List<GameEventListenerBase> listeners;
		/// <summary>The list of listeners for this event.</summary>
		private List<GameEventListenerBase> Listeners => this.listeners;

		/// <summary>Raises this event to all its listeners.</summary>
		[ContextMenu("Raise")]
        public void Raise()
        {
			// Iterate on list backwards in case you want unregister as part of the event
            for (int listenerIndex = this.Listeners.Count - 1; listenerIndex >= 0; listenerIndex--)
            { this.Listeners[listenerIndex].BaseOnEventRaised(); }
        }

		/// <summary>Adds a listener to the list.</summary>
		/// <param name="listener">The listener to add.</param>
        public void RegisterListener(GameEventListenerBase listener) => this.Listeners.Add(listener);

		/// <summary>Removes a listener from the list.</summary>
		/// <param name="listener">The listener to remove.</param>
        public void UnregisterListener(GameEventListenerBase listener) => this.Listeners.Remove(listener);
	}
}
