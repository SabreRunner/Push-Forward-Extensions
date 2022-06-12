
namespace PushForward.EventSystem
{
    using UnityEngine;

    /// <summary>An extension of the game event that contains a prefab.</summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Game Event Prefab", order = 26)]
    public class GameEventPrefab : GameEvent
    {
        public GameObject prefab;

        public void Raise(GameObject newPrefab)
        {
            this.prefab = newPrefab;
            this.Raise();
        }
    }
}