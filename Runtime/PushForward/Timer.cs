/*
 * Timer
 *
 * Description: General purpose timer class.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2018-11-15
*/

namespace PushForward
{
	#region using
	using System;
	using UnityEngine;
	using Base;
	using ExtensionMethods;
	#endregion // using

	public class Timer : BaseMonoBehaviour
	{
		/// <summary>How to display the timer.</summary>
		public enum DisplayMode { HHMMSS, HHMMSStt, MMSS, MMSStt, SS, SStt }

		#region Timer Action
		[Serializable]
		public class TimerEvent
		{
			/// <summary>Trigger event when timer drops to this amount of seconds.</summary>
			[SerializeField] private double triggerTimeInSeconds;
			/// <summary>The event to trigger.</summary>
			[SerializeField] private TimeSpanEvent timeSpanEvent;

			// "null value" time span
			private TimeSpan triggerTime = new TimeSpan(-1);

			/// <summary>Check/set this event has been triggered already</summary>
			public bool Triggered { get; set; }
			/// <summary>Get the trigger time for this event.</summary>
			public TimeSpan TriggerTime
			{
				get
				{
					if (this.triggerTime < TimeSpan.FromTicks(0))
					{ this.triggerTime = TimeSpan.FromSeconds(this.triggerTimeInSeconds); }
					
					return this.triggerTime;
				}
			}

			/// <summary>Get the event that needs to trigger.</summary>
			private TimeSpanEvent EventToTrigger => this.timeSpanEvent;

			// hide the basic constructor. Don't use that.
			private TimerEvent() { }

			/// <summary>Trigger the event (Send the trigger time with it).</summary>
			public void Trigger()
			{
				this.Triggered = true;
				this.EventToTrigger?.Invoke(this.TriggerTime);
			}

			/// <summary>Create a TimerEvent with a set trigger time and action to trigger.</summary>
			/// <param name="triggerTime">The time in which to trigger this event.</param>
			/// <param name="actionToTrigger">The action to trigger.</param>
			public static TimerEvent Create(TimeSpan triggerTime, TimeSpanEvent actionToTrigger)
			{ return new TimerEvent { triggerTime = triggerTime, timeSpanEvent = actionToTrigger }; }

			/// <summary>Create a timer event to trigger at timer's end.</summary>
			/// <remarks>While not setting a time, it is assumed this will trigger at the end.</remarks>
			/// <param name="actionToTrigger">The action to trigger</param>
			public static TimerEvent Create(TimeSpanEvent actionToTrigger)
			{ return Create(TimeSpan.FromTicks(0), actionToTrigger); }
		}
		#endregion // timer action

		#region Fields
		#pragma warning disable IDE0044 // Add readonly modifier
		/// <summary>The initial timer time.</summary>
		[SerializeField] private double timerInSeconds;
		/// <summary>How to display the timer time.</summary>
		[SerializeField] private DisplayMode displayMode;
		/// <summary>Where to output the timer time.</summary>
		[SerializeField] private UnityEngine.UI.Text outputText;
		/// <summary>The actions to take during the timer run.</summary>
		[SerializeField] private TimerEvent[] timerActions;
		/// <summary>The actions to take when the timer is over.</summary>
		[SerializeField] private TimerEvent[] timerOverActions;
		#pragma warning restore IDE0044 // Add readonly modifier

		// the actual timer data
		private TimeSpan time;
		#endregion // fields

		#region Setup
		/// <summary>Set Timer parameters.</summary>
		/// <param name="timerSpan">The time.</param>
		/// <param name="displayMode">How to display it.</param>
		/// <param name="timerActions">Actions to take in specific times.</param>
		/// <param name="timerOverActions">Actions to take when timer over.</param>
		public void Set(TimeSpan timerSpan, DisplayMode displayMode, TimerEvent[] timerActions, TimerEvent[] timerOverActions)
		{
			this.time = timerSpan;
			this.displayMode = displayMode;
			this.timerActions = timerActions;
			this.timerOverActions = timerOverActions;
		}

		/// <summary>Reset the timer to the given time span.</summary>
		/// <param name="timerSpan">The new time to set.</param>
		public void ResetTime(TimeSpan timerSpan)
		{
			this.PauseTimer();
			this.time = timerSpan;
		}
		#endregion // setup

		#region Timer
		/// <summary>Output the timer to the text component.</summary>
		private void OutputToText()
		{
			if (this.outputText == null)
			{ return; }
			
			string output;

			switch (this.displayMode)
			{
				case DisplayMode.HHMMSS:
					output = this.time.Hours + ":"
								+ this.time.Minutes.ToString("D2") + ":"
								+ this.time.Seconds.ToString("D2");
					break;
				default:
				case DisplayMode.HHMMSStt:
					output = this.time.Hours + ":"
								+ this.time.Minutes.ToString("D2") + ":"
								+ this.time.Seconds.ToString("D2") + "."
								+ this.time.Milliseconds.ToString("D3").Remove(2);
					break;
				case DisplayMode.MMSS:
					output = this.time.Minutes + ":" 
					            + this.time.Seconds.ToString("D2");
					break;
				case DisplayMode.MMSStt:
					output = this.time.Minutes + ":"
					            + this.time.Seconds.ToString("D2") + ":"
								+ this.time.Milliseconds.ToString("D3").Remove(2);
					break;
				case DisplayMode.SS: output = ((int)this.time.TotalSeconds).ToString(); break;
				case DisplayMode.SStt: output = this.time.TotalSeconds.ToString("N3"); break;
			}

			this.outputText.text = output;
		}

		/// <summary>Timer Update method.</summary>
		private void UpdateTimer()
		{
			// update timer
			this.time = this.time.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
			// make sure it doesn't dip below zero.
			this.time = this.time.Max(0);

			// show on text component
			this.OutputToText();

			// activate actions if available
			this.timerActions?.DoForEach(
				timerAction =>
				{
					if (!timerAction.Triggered && timerAction.TriggerTime >= this.time)
					{ timerAction.Trigger(); }
				});
		}

		/// <summary>Run all the actions that trigger at the end of the timer.</summary>
		private void TimerOverActions()
		{
			this.timerOverActions?.DoForEach(timerAction => { timerAction.Trigger(); });
		}

		/// <summary>Start the timer.</summary>
		[ContextMenu("Start Timer")]
		public void StartTimer()
		{
			// define the predicate for when the timer should be running
			bool TimerRunningPredicate() => this.time.Ticks > 0;
			// run the timer update while predicate is true
			this.ActionEachFrameWhilePredicate(this.UpdateTimer, TimerRunningPredicate);
			// run the timer over actions when predicate is false
			this.ActionWhenPredicate(this.TimerOverActions, () => !TimerRunningPredicate());
		}

		/// <summary>Stop all timer functions. Time remaining is NOT reset.</summary>
		[ContextMenu("Pause Timer")]
		public void PauseTimer()
		{
			this.StopAllCoroutines();
		}
		#endregion // timer

		#region Engine
		private void Awake()
		{
			// set up timer from inspector
			this.time = TimeSpan.FromSeconds(this.timerInSeconds);
		}
		#endregion // engine
	}
}
