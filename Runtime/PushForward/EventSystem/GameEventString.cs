namespace PushForward.EventSystem
{
    using UnityEngine;

    /// <summary>An extension of the game event that contains a boolean.</summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Game Event String", order = 27)]
    class GameEventString : GameEvent
    {
        public string @string;

        public void Raise(string newString)
        {
            this.@string = newString;
            this.Raise();
        }
    }
}