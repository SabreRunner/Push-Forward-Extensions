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

	public class Timer : MonoBehaviour
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
			private TimeSpan triggerTime = new(-1);

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
				=> new() { triggerTime = triggerTime, timeSpanEvent = actionToTrigger };

			/// <summary>Create a timer event to trigger at timer's end.</summary>
			/// <remarks>While not setting a time, it is assumed this will trigger at the end.</remarks>
			/// <param name="actionToTrigger">The action to trigger</param>
			public static TimerEvent Create(TimeSpanEvent actionToTrigger) => Create(TimeSpan.FromTicks(0), actionToTrigger);
		}
		#endregion // timer action

		#region Fields
		#pragma warning disable IDE0044 // Add readonly modifier
		/// <summary>The initial timer time.</summary>
		[SerializeField] private double timerInSeconds = -1;
		/// <summary>How to display the timer time.</summary>
		[SerializeField] private DisplayMode displayMode;
		/// <summary>Reciever for timer output as double (in milliseconds).</summary>
		[SerializeField] private DoubleEvent outputDouble;
		/// <summary>Reciever for timer output as text.</summary>
		[SerializeField] private StringEvent outputText;
		/// <summary>The actions to take during the timer run.</summary>
		[SerializeField] private TimerEvent[] timerActions;
		/// <summary>The actions to take when the timer is over.</summary>
		[SerializeField] private TimerEvent[] timerOverActions;
		#pragma warning restore IDE0044 // Add readonly modifier

		/// <summary>The actual timer data</summary>
		private TimeSpan time;
		#endregion // fields

		#region Setup
		/// <summary>Set Timer parameters.</summary>
		/// <param name="timerSpan">The time.</param>
		/// <param name="newDisplayMode">How to display it.</param>
		/// <param name="newTimerActions">Actions to take in specific times.</param>
		/// <param name="newTimerOverActions">Actions to take when timer over.</param>
		public void Set(TimeSpan timerSpan, DisplayMode newDisplayMode, TimerEvent[] newTimerActions, TimerEvent[] newTimerOverActions)
		{
			this.time = timerSpan;
			this.displayMode = newDisplayMode;
			this.timerActions = newTimerActions;
			this.timerOverActions = newTimerOverActions;
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
		private const string TimeStringDelimiter = ":";
		private const string Time2Digits = "D2";
		private const string Time3Digits = "D3";
		/// <summary>Output the timer to the reciever.</summary>
		private void OutputToRecievers()
		{
			this.outputDouble?.Invoke(this.time.TotalMilliseconds);

			if (this.outputText == null)
			{ return; }

			string output = this.displayMode switch
				{
					DisplayMode.HHMMSS => this.time.Hours + Timer.TimeStringDelimiter + this.time.Minutes.ToString(Timer.Time2Digits)
										  + Timer.TimeStringDelimiter + this.time.Seconds.ToString(Timer.Time2Digits),
					DisplayMode.MMSS => this.time.Minutes + Timer.TimeStringDelimiter + this.time.Seconds.ToString(Timer.Time2Digits),
					DisplayMode.MMSStt => this.time.Minutes + Timer.TimeStringDelimiter + this.time.Seconds.ToString(Timer.Time2Digits)
										  + Timer.TimeStringDelimiter + this.time.Milliseconds.ToString(Timer.Time3Digits).Remove(2),
					DisplayMode.SS => ((int)this.time.TotalSeconds).ToString(),
					DisplayMode.SStt => this.time.TotalSeconds.ToString("N3"),
					_ => this.time.Hours + Timer.TimeStringDelimiter + this.time.Minutes.ToString(Timer.Time2Digits)
						 + Timer.TimeStringDelimiter + this.time.Seconds.ToString(Timer.Time2Digits) + "." +
						 this.time.Milliseconds.ToString(Timer.Time3Digits).Remove(2)
				};

			this.outputText?.Invoke(output);
		}

		/// <summary>Timer Update method.</summary>
		private void UpdateTimer()
		{
			// update timer
			this.time = this.time.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
			// make sure it doesn't dip below zero.
			this.time = this.time.Max(0);

			this.OutputToRecievers();

			// activate actions if available
			this.timerActions?.DoForEach(timerAction =>
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
			this.ActionEachFrameWhilePredicate(TimerRunningPredicate, this.UpdateTimer);
			// run the timer over actions when predicate is false
			this.ActionWhenPredicate(() => !TimerRunningPredicate(), this.TimerOverActions);
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
