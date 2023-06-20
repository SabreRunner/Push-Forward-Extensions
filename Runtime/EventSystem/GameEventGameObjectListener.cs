/*
	GameEventGameObjectListener

	Description: The receiver for GameEventPrefabs

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.EventSystem
{
    using Base;
    using UnityEngine;
	using UnityEngine.Serialization;

	public class GameEventGameObjectListener : GameEventListenerBase
    {
        /// <summary>This listener's event is an event with a prefab.</summary>
        [FormerlySerializedAs("gameEventPrefab"),SerializeField] private GameEventGameObject gameEventGameObject;
		// ReSharper disable once UnassignedField.Global -- assigned, if needed, at user runtime.
		private EventGetter<GameEventGameObject> gameObjectEventGetter;

		public override GameEvent GameEvent => this.gameEventGameObject != null ? this.gameEventGameObject
												   : this.gameEventGameObject = this.GameObjectEventGetter?.GetEventAction();
		public EventGetter<GameEventGameObject> GameObjectEventGetter
		{
			get => this.gameObjectEventGetter;
			set
			{
				Object.Destroy(this.gameEventGameObject);
				this.gameEventGameObject = null;
				this.gameObjectEventGetter = value;
			}
		}

		/// <summary>This listener's event gets a prefab.</summary>
        [FormerlySerializedAs("prefabResponse"),SerializeField] private GameObjectEvent gameObjectResponse;

        protected override void OnEventRaised()
        { this.gameObjectResponse?.Invoke(this.gameEventGameObject.gameObject); }
    }

}