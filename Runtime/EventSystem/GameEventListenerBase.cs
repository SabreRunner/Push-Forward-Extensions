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

	// ReSharper disable InvalidXmlDocComment

	/// <summary>A component to grab the specific event for the listener, if it can't be assigned in advance.</summary>
	/// <remarks>How to Use:
	///		1. Inherit this with the type of event you want to get and name the new class after the specific event you're getting.
	///		2. Create an EventInjector ScriptableObject.
	///		3. Create serialised fields for all the events you want to inject and assign them.
	///		4. Create backing fields for the Getters you need for these events.
	///		5. Give it a static Instance property to retrieve the EventInjector and assign to it OnEnable.
	///		6. When you need the event getter you call "[DependencyInjectorClassName].Instance.[SpecificEventGetterPropertyName].
	///		7. From that you can also get the event directly.
	///
	///		We need all of this to keep the Base Listener abstract and inheritable as it is and the inherited listeners simple.</remarks>
	/// <example>
	///		public class InstantiationEventGetter : EventGetter<GameEventInstantiation>
	///		{ public InstantiationEventGetter(Func<GameEventInstantiation> eventToGet) : base(eventToGet) { } }
	///
	///		public class EventInjector : ScriptableObject
	///		{
	///			[SerializeField, Tooltip("The Instantiation event to use in the game.")] private GameEventInstantiation instantiationEvent;
	///
	/// 		private InstantiationEventGetter instantiationEventGetter;
	///
	/// 		public static EventInjector Instance { get; private set; }
	///
	/// 		public static InstantiationEventGetter InstantiationEventGetter
	///				=> EventInjector.Instance.instantiationEventGetter
	///						?? (EventInjector.Instance.instantiationEventGetter
	///							= new InstantiationEventGetter(() => EventInjector.Instance.instantiationEvent));
	///
	///			private void OnEnable() => EventInjector.Instance = this;
	///		}
	/// </example>
	public abstract class EventGetter<TGameEvent> where TGameEvent : GameEvent
	{
		public Func<TGameEvent> GetEventAction { private set; get; }
		public EventGetter(Func<TGameEvent> getEventAction) => this.GetEventAction = getEventAction;
	}
	// ReSharper restore InvalidXmlDocComment

	/// <summary>The base game event listener.
	///		Defines the Game Event property, the Game Event Getter and the on event raised method.
	///		handles registration and unregistration.</summary>
	public abstract class GameEventListenerBase : MonoBehaviour
	{
		[SerializeField, Tooltip("Whether to respond to events even if the object is disabled.")] protected bool respondWhenDisabled;
		/// <summary>Every listener must have its event. An abstract property allows casting from any derived extensions.</summary>
		/// <remarks>With the addition of Event Getters, they are also not specifically mentioned here but need to be used
		///		as part of this property. Create one, create a field for one, and use it if you can't assign an event directly.</remarks>
		public abstract GameEvent GameEvent { get; }

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