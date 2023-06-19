/*
	GameEventInt

	Description: A game event for sending ints

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
	using UnityEngine;

	/// <summary>An extension of the game event that contains an int.</summary>
	[CreateAssetMenu(menuName = "Event System/Game Event Integer", order = 21)]
	public class GameEventInt : GameEvent
	{
		public int integer;

		public void Raise(int newInteger)
		{
			this.integer = newInteger;
			this.Raise();
		}
	}
}
