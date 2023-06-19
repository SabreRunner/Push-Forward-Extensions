
namespace PushForward.EventSystem
{
    using UnityEngine;

    /// <summary>An extension of the game event that contains a prefab.</summary>
    [CreateAssetMenu(menuName = "Event System/Game Event Game Object", order = 26)]
    public class GameEventGameObject : GameEvent
    {
        public GameObject gameObject;

        public void Raise(GameObject newGameObject)
        {
            this.gameObject = newGameObject;
            this.Raise();
        }
    }
}