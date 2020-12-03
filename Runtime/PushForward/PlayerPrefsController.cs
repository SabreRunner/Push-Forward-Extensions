
namespace PushForward
{
	#region using
	using UnityEngine;
	using UnityEngine.Events;
	using PushForward.Base;
	#endregion // using

	public class PlayerPrefsController : BaseMonoBehaviour
	{
		#region player prefs invoke
		[System.Serializable]
		public struct PlayerPrefsInvoke
		{
			public enum ExecutionOrder { None, Awake, Enabled }

			public ExecutionOrder whenToRunThis;
			public string key;

			public void Invoke(UnityEvent existsEvent, StringEvent stringEvent, IntEvent intEvent, FloatEvent floatEvent)
			{
				if (this.key.IsNullOrEmpty())
				{ return; }

				if (PlayerPrefs.HasKey(this.key))
				{ existsEvent.Invoke(); }

				Debug.Log(PlayerPrefs.GetString(this.key));
				Debug.Log(PlayerPrefs.GetInt(this.key));
				Debug.Log(PlayerPrefs.GetFloat(this.key));
			}
		}
		#endregion // player prefs invoke

		#region player prefs saver
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
		[SerializeField] private PlayerPrefsInvoke playerPrefsInvoker;
		[SerializeField] private PlayerPrefsSaver playerPrefsSaver;
		[SerializeField] private UnityEvent existsEvent;
		[SerializeField] private StringEvent stringEvent;
		[SerializeField] private IntEvent intEvent;
		[SerializeField] private FloatEvent floatEvent;
		#endregion // fields

		#region invoke
		public void Invoke()
		{
			this.playerPrefsInvoker.Invoke(this.existsEvent, this.stringEvent, this.intEvent, this.floatEvent);
		}

		public void InvokeNone()
		{
			if (this.playerPrefsInvoker.whenToRunThis == PlayerPrefsInvoke.ExecutionOrder.None)
			{ playerPrefsInvoker.Invoke(this.existsEvent, this.stringEvent, this.intEvent, this.floatEvent); }
		}

		public void InvokeAwake()
		{
			if (this.playerPrefsInvoker.whenToRunThis == PlayerPrefsInvoke.ExecutionOrder.Awake)
			{ this.playerPrefsInvoker.Invoke(this.existsEvent, this.stringEvent, this.intEvent, this.floatEvent);
			}
		}

		public void InvokeEnabled()
		{
			if (this.playerPrefsInvoker.whenToRunThis == PlayerPrefsInvoke.ExecutionOrder.Enabled)
			{ this.playerPrefsInvoker.Invoke(this.existsEvent, this.stringEvent, this.intEvent, this.floatEvent); }
		}
		#endregion // invoke

		public void Save()
		{
			this.playerPrefsSaver.Save();
		}

		[ContextMenu("Delete All")]
		public void DeleteAll()
		{ PlayerPrefs.DeleteAll(); }

		#region engine
		private void Awake()
		{ this.InvokeAwake(); }

		private void OnEnable()
		{ this.InvokeEnabled(); }
		#endregion // engine
	}
}
