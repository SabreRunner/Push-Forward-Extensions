/*
	GameEventFloat

	Description: A Game event for sending floats

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
	using UnityEngine;

	/// <summary>An extension of the game event that contains an int.</summary>
	[CreateAssetMenu(menuName = "Event System/Game Event Float", order = 31)]
	public class GameEventFloat : GameEvent
	{
		public float @float;

		public void Raise(float newFloat)
		{
			this.@float = newFloat;
			this.Raise();
		}
	}
}
