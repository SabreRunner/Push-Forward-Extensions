/*
	GameEventString

	Description: The game event for sending strings.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
    using UnityEngine;

    /// <summary>An extension of the game event that contains a boolean.</summary>
    [CreateAssetMenu(menuName = "Event System/Game Event String", order = 27)]
    public class GameEventString : GameEvent
    {
        public string @string;

        public void Raise(string newString)
        {
            this.@string = newString;
            this.Raise();
        }
    }
}