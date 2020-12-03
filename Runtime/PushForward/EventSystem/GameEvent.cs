
namespace PushForward.EventSystem
{
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>The very basic game event.
	///		Just registers listeners and raises events.</summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Game Event", order = 20)]
    public class GameEvent : ScriptableObject
    {
		/// <summary>The list of listeners for this event.</summary>
        protected readonly List<GameEventListenerBase> listeners = new List<GameEventListenerBase>();

		/// <summary>Raises this event to all its listeners.</summary>
        [ContextMenu("Raise")]
        public void Raise()
        {
            for (int listenerIndex = this.listeners.Count - 1; listenerIndex >= 0; listenerIndex--)
            { this.listeners[listenerIndex].BaseOnEventRaised(); }
        }

		/// <summary>Adds a listener to the list.</summary>
		/// <param name="listener">The listener to add.</param>
        public void RegisterListener(GameEventListenerBase listener)
        { this.listeners.Add(listener); }

		/// <summary>Removes a listener from the list.</summary>
		/// <param name="listener">The listener to remove.</param>
        public void UnregisterListener(GameEventListenerBase listener)
        { this.listeners.Remove(listener); }
    }
}
