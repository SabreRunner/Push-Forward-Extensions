/*
	Trigger At

	Description: A multi use system for triggering public methods automatically under specific conditions.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2022-11-22
*/


namespace PushForward.Base
{
	using System;
	using UnityEngine;
	using UnityEngine.Events;

	public class TriggerAt : MonoBehaviour
	{
		#region enums
		// ReSharper disable UnusedMember.Local
		public enum TriggerPoint { Never = 0, Awake, Start, Enabled, Disabled, Destroyed, Update,
									CollisionEnter, CollisionStay, CollisionExit, TriggerEnter, TriggerStay, TriggerExit }

		// ReSharper disable once InconsistentNaming
		public enum TriggerPlatform { All = 0, Standalone, Android, iOS, Editor }
		// ReSharper restore UnusedMember.Local
		#endregion //enums

		#region fields
		#pragma warning disable IDE0044 // Add readonly modifier
		[SerializeField, Tooltip("Under what condition to trigger.")] private TriggerPoint triggerPoint;
		[SerializeField, Tooltip("Under which OS to trigger.")] private TriggerPlatform triggerPlatform;
		[SerializeField, Tooltip("Delay the trigger by seconds.")] private float triggerDelayInSeconds;
		[SerializeField, Tooltip("The event to trigger.")] private UnityEvent triggerEvent;
		#pragma warning restore IDE0044 // Add readonly modifier
		#endregion //fields

		public void SetTrigger(TriggerPoint newPoint, Action newEvent, float newDelay = -1f, TriggerPlatform newPlatform = TriggerPlatform.All)
		{
			this.triggerPoint = newPoint;
			this.triggerPlatform = newPlatform;
			this.triggerEvent = new UnityEvent();
			this.triggerEvent.AddListener(()=>newEvent());
		}

		[ContextMenu("Trigger")]
		public void Trigger()
		{
			#if UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_EDITOR
			// { this.Temp("Platform: " + Application.platform); }

			switch (this.triggerPlatform)
			{
				case TriggerPlatform.Standalone: if (Application.platform is not RuntimePlatform.LinuxPlayer and not RuntimePlatform.OSXPlayer
																			and not RuntimePlatform.WindowsPlayer) { return; } break;
				case TriggerPlatform.Android: if (Application.platform is not RuntimePlatform.Android and not RuntimePlatform.WindowsEditor)
												{ return; } break;
				case TriggerPlatform.iOS: if (Application.platform is not RuntimePlatform.IPhonePlayer and not RuntimePlatform.OSXEditor)
											{ return; } break;
				case TriggerPlatform.Editor: if (Application.platform is not RuntimePlatform.LinuxEditor and not RuntimePlatform.WindowsEditor
																		and not RuntimePlatform.OSXEditor) { return; } break;
			}
			#endif
			// { this.Temp("Triggering " + this.triggerEvent.GetPersistentMethodName(0) + " in " + this.triggerDelayInSeconds + " seconds."); }

			if (this.triggerDelayInSeconds < float.Epsilon)
			{ this.triggerEvent.Invoke(); }
			else { this.ActionInSeconds(this.triggerDelayInSeconds, this.triggerEvent.Invoke); }
		}

		public void AbortTrigger() => this.StopAllCoroutines();

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
		private void OnCollisionEnter2D(Collision2D collision) => this.TriggerOn(TriggerPoint.CollisionEnter);
		private void OnCollisionStay(Collision collision) => this.TriggerOn(TriggerPoint.CollisionStay);
		private void OnCollisionStay2D(Collision2D collision) => this.TriggerOn(TriggerPoint.CollisionStay);
		private void OnCollisionExit(Collision other) => this.TriggerOn(TriggerPoint.CollisionExit);
		private void OnCollisionExit2D(Collision2D other) => this.TriggerOn(TriggerPoint.CollisionExit);
		private void OnTriggerEnter(Collider other) => this.TriggerOn(TriggerPoint.TriggerEnter);
		private void OnTriggerEnter2D(Collider2D other) => this.TriggerOn(TriggerPoint.TriggerEnter);
		private void OnTriggerStay(Collider other) => this.TriggerOn(TriggerPoint.TriggerStay);
		private void OnTriggerStay2D(Collider2D other) => this.TriggerOn(TriggerPoint.TriggerStay);
		private void OnTriggerExit(Collider other) => this.TriggerOn(TriggerPoint.TriggerExit);
		private void OnTriggerExit2D(Collider2D other) => this.TriggerOn(TriggerPoint.TriggerExit);
		#endregion // engine
	}
}
