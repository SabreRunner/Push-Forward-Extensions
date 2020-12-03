
namespace PushForward.EventSystem
{
    using UnityEngine;

    /// <summary>An extension of the game event that contains a boolean.</summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Game Event Bool", order = 22)]
    class GameEventBool : GameEvent
    {
        public bool condition;

        public void Raise(bool newCondition)
        {
            this.condition = newCondition;
            this.Raise();
        }
    }
}
