/*
 * Player Prefs Controller
 *
 * Description: A controller that can be used to change PlayerPrefs more easily and also to get notifications about PlayerPrefs.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-12
*/

namespace PushForward
{
	#region using
	using System.Text;
	using UnityEngine;
	using UnityEngine.Events;
	using Base;
	#endregion // using

	public class PlayerPrefsController : MonoBehaviour
	{
		#region player prefs invoke
		/// <summary>A struct to be used to register to get PlayerPrefs Values</summary>
		/// <summary>The value type aligns with the events added.</summary>
		[System.Serializable]
		public struct PlayerPrefsInvoker
		{
			public enum ExecutionOrder { Unknown, Awake, Enabled, OnDemand }

			/// <summary>When should the event trigger.</summary>
			public ExecutionOrder whenToRunThis;
			/// <summary>The key to check.</summary>
			public string key;

			/// <summary>What to trigger when this is invoked.</summary>
			/// <param name="existsEvent">Whether the value exists.</param>
			/// <param name="stringEvent">The event to invoke if this is a string.</param>
			/// <param name="intEvent">The event to invoke if this is an int.</param>
			/// <param name="floatEvent">The event to invoke if this is a float.</param>
			public void Invoke(UnityEvent existsEvent, StringEvent stringEvent, IntEvent intEvent, FloatEvent floatEvent)
			{
				if (this.key.IsNullOrEmpty())
				{ return; }

				if (PlayerPrefs.HasKey(this.key))
				{ existsEvent.Invoke(); }

				stringEvent?.Invoke(PlayerPrefs.GetString(this.key));
				intEvent?.Invoke(PlayerPrefs.GetInt(this.key));
				floatEvent?.Invoke(PlayerPrefs.GetFloat(this.key));
			}
		}
		#endregion // player prefs invoke

		#region player prefs saver
		/// <summary>A struct to create a PlayerPrefs value to save.</summary>
		/// <remarks>ASSERT: The value type aligns with the value set.</remarks>
		[System.Serializable]
		public struct PlayerPrefsSaver
		{
			public enum ValueType { String, Int, Float }

			public string key;
			public ValueType valueType;
			public string stringValue;
			public int intValue;
			public float floatValue;

			public void Save()
			{
				if (this.key.IsNullOrEmpty())
				{ return; }

				switch (this.valueType)
				{
					case ValueType.String: PlayerPrefs.SetString(this.key, this.stringValue); return;
					case ValueType.Int: PlayerPrefs.SetInt(this.key, this.intValue); return;
					case ValueType.Float: PlayerPrefs.SetFloat(this.key, this.floatValue); return;
				}
			}
		}
		#endregion // player prefs saver

		#region fields
		[SerializeField] private PlayerPrefsInvoker playerPrefsInvoker;
		[SerializeField] private PlayerPrefsSaver playerPrefsSaver;
		[SerializeField] private UnityEvent existsEvent;
		[SerializeField] private StringEvent stringEvent;
		[SerializeField] private IntEvent intEvent;
		[SerializeField] private FloatEvent floatEvent;
		#endregion // fields

		#region invoke
		public void Invoke() => this.playerPrefsInvoker.Invoke(this.existsEvent, this.stringEvent, this.intEvent, this.floatEvent);

		public void InvokeNone()
		{
			if (this.playerPrefsInvoker.whenToRunThis == PlayerPrefsInvoker.ExecutionOrder.Unknown)
			{ this.playerPrefsInvoker.Invoke(this.existsEvent, this.stringEvent, this.intEvent, this.floatEvent); }
		}

		public void InvokeAwake()
		{
			if (this.playerPrefsInvoker.whenToRunThis == PlayerPrefsInvoker.ExecutionOrder.Awake)
			{ this.playerPrefsInvoker.Invoke(this.existsEvent, this.stringEvent, this.intEvent, this.floatEvent); }
		}

		public void InvokeEnabled()
		{
			if (this.playerPrefsInvoker.whenToRunThis == PlayerPrefsInvoker.ExecutionOrder.Enabled)
			{ this.playerPrefsInvoker.Invoke(this.existsEvent, this.stringEvent, this.intEvent, this.floatEvent); }
		}
		#endregion // invoke

		public void Save() => this.playerPrefsSaver.Save();

		[ContextMenu("Delete All")]
		public void DeleteAll() => PlayerPrefs.DeleteAll();

		#region engine
		private void Awake() => this.InvokeAwake();

		private void OnEnable() => this.InvokeEnabled();
		#endregion // engine
	}
}
