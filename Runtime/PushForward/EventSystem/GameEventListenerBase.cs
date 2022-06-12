/*
	GameEventListenerBase

	Description: The abstract base class for all game event listeners.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
	using UnityEngine;

	/// <summary>The base game event listener.
	///		Defines the game event and the event raised method.
	///		handles registration and unregistration.</summary>
	public abstract class GameEventListenerBase : MonoBehaviour
	{
		[SerializeField] protected bool respondWhenDisabled;
		/// <summary>Every listener must have its event.
		///		an abstract property allows casting from any derived extensions.</summary>
		protected abstract GameEvent GameEvent { get; }

		private void OnEnable()
		{
			GameEvent gameEvent = this.GameEvent;
			if (gameEvent != null)
			{ gameEvent.RegisterListener(this); }
		}

		private void OnDisable()
		{
			if (!this.respondWhenDisabled)
			{
				GameEvent gameEvent = this.GameEvent;
				if (gameEvent != null)
				{ gameEvent.UnregisterListener(this); }
			}
		}

		/// <summary>Responds to the event being raised.
		///		An abstract method allows each derived listener to do it differently.</summary>
		protected abstract void OnEventRaised();

		public void BaseOnEventRaised()
		{
			if (!this.enabled && !this.respondWhenDisabled)
			{ return; }

			this.OnEventRaised();
		}
	}
}