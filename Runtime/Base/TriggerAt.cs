namespace PushForward
{
	using System;
	using UnityEngine;
	using UnityEngine.Events;

	public class TriggerAt : MonoBehaviour
	{
		#region enums
		// ReSharper disable UnusedMember.Local
		private enum TriggerPoint { Never = 0, Awake, Start, Enabled, Disabled, Destroyed, Update,
									CollisionEnter, CollisionStay, CollisionExit, TriggerEnter, TriggerStay, TriggerExit }

		// ReSharper disable once InconsistentNaming
		private enum TriggerPlatform { All = 0, Standalone, Android, iOS, Editor }
		// ReSharper restore UnusedMember.Local
		#endregion //enums

		#region fields
#pragma warning disable IDE0044 // Add readonly modifier
		[SerializeField] private TriggerPoint triggerPoint;
		[SerializeField] private TriggerPlatform triggerPlatform;
		[SerializeField] private float triggerDelayInSeconds;
#if DEBUG
		[SerializeField] private bool debugLog;
#endif
		[SerializeField] private UnityEvent triggerEvent;
#pragma warning restore IDE0044 // Add readonly modifier
		#endregion //fields

		[ContextMenu("Trigger")]
		public void Trigger()
		{
			#if UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_EDITOR
			// { this.Temp("Platform: " + Application.platform); }

			switch (this.triggerPlatform)
			{
				case TriggerPlatform.Standalone:
					if (Application.platform != RuntimePlatform.LinuxPlayer
						&& Application.platform != RuntimePlatform.OSXPlayer
						&& Application.platform != RuntimePlatform.WindowsPlayer)
					{ return; }
					break;
				case TriggerPlatform.Android:
					if (Application.platform != RuntimePlatform.Android
						&& Application.platform != RuntimePlatform.WindowsEditor)
					{ return; }
					break;
				case TriggerPlatform.iOS:
					if (Application.platform != RuntimePlatform.IPhonePlayer
						&& Application.platform != RuntimePlatform.OSXEditor)
					{ return; }
					break;
				case TriggerPlatform.Editor:
					if (Application.platform != RuntimePlatform.LinuxEditor
						&& Application.platform != RuntimePlatform.WindowsEditor
						&& Application.platform != RuntimePlatform.OSXEditor)
					{ return; }
					break;
			}
			#endif
			// { this.Temp("Triggering " + this.triggerEvent.GetPersistentMethodName(0) + " in " + this.triggerDelayInSeconds + " seconds."); }

			if (this.triggerDelayInSeconds < float.Epsilon)
			{ this.triggerEvent.Invoke(); }
			else { this.ActionInSeconds(this.triggerEvent.Invoke, this.triggerDelayInSeconds); }
		}

		public void AbortTrigger()
		{
			this.StopAllCoroutines();
		}

		#region engine
		private void TriggerOn(TriggerPoint point)
		{
			if (this.triggerPoint == point)
			{ this.Trigger(); }
		}

		private void Awake() => this.TriggerOn(TriggerPoint.Awake);
		private void Start() => this.TriggerOn(TriggerPoint.Start);
		private void OnEnable() => this.TriggerOn(TriggerPoint.Enabled);
		private void OnDisable() => this.TriggerOn(TriggerPoint.Disabled);
		private void OnDestroy() => this.TriggerOn(TriggerPoint.Destroyed);
		private void Update() => this.TriggerOn(TriggerPoint.Update);
		private void OnCollisionEnter(Collision collision) => this.TriggerOn(TriggerPoint.CollisionEnter);
		private void OnCollisionEnter2D(Collision2D col) => this.TriggerOn(TriggerPoint.CollisionEnter);
		private void OnCollisionStay(Collision collisionInfo) => this.TriggerOn(TriggerPoint.CollisionStay);
		private void OnCollisionStay2D(Collision2D collision) => this.TriggerOn(TriggerPoint.CollisionStay);
		private void OnCollisionExit(Collision other) => this.TriggerOn(TriggerPoint.CollisionExit);
		private void OnCollisionExit2D(Collision2D other) => this.TriggerOn(TriggerPoint.CollisionExit);
		private void OnTriggerEnter(Collider col) => this.TriggerOn(TriggerPoint.TriggerEnter);
		private void OnTriggerEnter2D(Collider2D other) => this.TriggerOn(TriggerPoint.TriggerEnter);
		private void OnTriggerStay(Collider col) => this.TriggerOn(TriggerPoint.TriggerStay);
		private void OnTriggerStay2D(Collider2D other) => this.TriggerOn(TriggerPoint.TriggerStay);
		private void OnTriggerExit(Collider col) => this.TriggerOn(TriggerPoint.TriggerExit);
		private void OnTriggerExit2D(Collider2D other) => this.TriggerOn(TriggerPoint.TriggerExit);
		#endregion // engine
	}
}
