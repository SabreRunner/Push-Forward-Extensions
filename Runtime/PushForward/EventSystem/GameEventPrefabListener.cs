/*
	GameEventPrefabListener

	Description: The receiver for GameEventPrefabs

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
    using Base;
    using UnityEngine;

    public class GameEventPrefabListener : GameEventListenerBase
    {
        /// <summary>This listener's event is an event with a prefab.</summary>
        [SerializeField] private GameEventPrefab gameEventPrefab;
        protected override GameEvent GameEvent => this.gameEventPrefab;
        /// <summary>This listener's event gets a prefab.</summary>
        [SerializeField] private PrefabEvent prefabResponse;

        protected override void OnEventRaised()
        { this.prefabResponse?.Invoke(this.gameEventPrefab.prefab); }
    }

}