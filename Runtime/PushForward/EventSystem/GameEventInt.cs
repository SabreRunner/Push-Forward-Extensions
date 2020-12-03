
namespace PushForward.EventSystem
{
	using UnityEngine;

	/// <summary>An extension of the game event that contains an int.</summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/Game Event Integer", order = 21)]
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
