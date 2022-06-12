/*
	GameEventStringListener

	Description: The receiver for GameEventStrings.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
    using Base;
    using UnityEngine;

    public class GameEventStringListener : GameEventListenerBase
    {
        /// <summary>This listener's event is an event with a number.</summary>
        [SerializeField] private GameEventString gameEventString;
        protected override GameEvent GameEvent => this.gameEventString;
        /// <summary>This listener's event gets an integer.</summary>
        [SerializeField] private StringEvent eventResponse;

        protected override void OnEventRaised()
        { this.eventResponse?.Invoke(this.gameEventString.@string); }
    }
}