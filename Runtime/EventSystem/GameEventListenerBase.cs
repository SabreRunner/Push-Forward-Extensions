/*
	GameEventListenerBase

	Description: The abstract base class for all game event listeners.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
	using System;
	using UnityEngine;

	/// <summary>A component to grab the specific event for the listener, if it can't be assigned in advance.</summary>
	/// <remarks>How to Use:
	///		1. Inherit this with the type of event you want to get and name the new class after the specific event you're getting.
	///		2. Create an EventInjector ScriptableObject.
	///		3. Create serialised fields for all the events you want to inject.
	///		4. Create backing fields for the Getters you need for these events.
	///		5. Give it static Properties to retrieve the specific events you need and use Lazy Evaluation to create the getter if it's not instanced.
	///		6. When you need the event you call "[DependencyInjector].[SpecificEvent]</remarks>
	public abstract class EventGetter<TGameEvent> where TGameEvent : GameEvent
	{
		public Func<TGameEvent> GetEventAction { private set; get; }
		public EventGetter(Func<TGameEvent> getEventAction) => this.GetEventAction = getEventAction;
	}

	/// <summary>The base game event listener.
	///		Defines the game event and the event raised method.
	///		handles registration and unregistration.</summary>
	public abstract class GameEventListenerBase : MonoBehaviour
	{
		[SerializeField, Tooltip("Whether to respond to events even if the object is disabled.")] protected bool respondWhenDisabled;
		[Tooltip("The class to use to get the event.")] public EventGetter<GameEvent> gameEventGetter;
		/// <summary>Every listener must have its event.
		///		an abstract property allows casting from any derived extensions.</summary>
		public abstract GameEvent GameEvent { get; }

		private void OnEnable()
		{
			GameEvent gameEvent = this.GameEvent;
			if (gameEvent == null && this.gameEventGetter != null)
			{ gameEvent = this.gameEventGetter.GetEventAction(); }
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