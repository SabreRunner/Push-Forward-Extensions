
namespace PushForward.EventSystem
{
	using UnityEngine;

	/// <summary>An extension of the game event that contains an int.</summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/Game Event Float", order = 31)]
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
