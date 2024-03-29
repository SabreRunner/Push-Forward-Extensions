/*
	AnimatorExtender

	Description: Extends the Animator class.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.Extenders
{
	#region using
	using System;
	using UnityEngine;
	using UnityEngine.Events;
	using ExtensionMethods;
	#endregion // using

	[RequireComponent(typeof(Animator))]
	public class AnimatorExtender : MonoBehaviour
	{
		#region Inspector Fields
		[SerializeField] private Animator animator;
		#endregion // inspector fields

		#region Properties
		private int CurrentStateHash => this.animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
		// ReSharper disable once UnusedMember.Local
		private float CurrentStateNormalisedTime => this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		#endregion // properties

		#region Private Fields
		private float savedSpeed;
		#endregion // private fields

		#region animation events
		/// <summary>A description of an animation event.</summary>
		/// <remarks>Used to avoid just having events named Element 0.</remarks>
		[Serializable]
		public struct AnimationEvent
		{
			public string eventName;
			public UnityEvent eventInvoker;

			public void Invoke()
			{ this.eventInvoker.Invoke(); }
		}

		[Tooltip("Enter events you want triggered here. Then call them from inside the animation.")]
		[SerializeField] private AnimationEvent[] animationEventArray;

		public void InvokeEvent(int eventNumber)
		{
			if (!eventNumber.Between(0, this.animationEventArray.Length - 1))
			{ return; }

			this.animationEventArray[eventNumber].Invoke();
		}
		#endregion // animation events

		#region Speed Control
		public void ZeroSpeed()
		{
			this.savedSpeed = this.animator.speed;
			this.animator.speed = 0;
		}
		public void NormalSpeed()
		{
			this.savedSpeed = this.animator.speed;
			this.animator.speed = 1;
		}
		public void ResumeSpeed()
		{
			this.animator.speed = this.savedSpeed;
		}
		#endregion // speed control

		public void SeekAnimation(float normalisedTime)
		{
			this.animator.Play(this.CurrentStateHash, 0, normalisedTime);
			this.animator.speed = 0;
		}

		#region engine
		private void OnValidate()
		{
			this.animator = this.GetComponent<Animator>();
		}
		#endregion // engine
	}
}