/*
 * PredicateTrigger
 *
 * Description: A connective component between complicated outputs, including some simple validation, and simple specific events/calls. 
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2022-06-12
*/

using UnityEngine;
using UnityEngine.Events;

namespace PushForward.Base
{
	public class PredicateTrigger : MonoBehaviour
	{
		[SerializeField, Tooltip("Invoked when predicate is true.")] private UnityEvent predicateTrueEvent;
		[SerializeField, Tooltip("Invoked when predicate is false.")] private UnityEvent predicateFalseEvent;

		public bool Toggle { get; set; }

		public void PredicateStringIsNullOrEmpty(string str)
		{
			if (string.IsNullOrEmpty(str))
			{ this.predicateTrueEvent.Invoke(); }
			else { this.predicateFalseEvent.Invoke(); }
		}

		public void PredicateToggle()
		{
			this.Toggle = !this.Toggle;
			if (this.Toggle)
			{ this.predicateTrueEvent.Invoke(); }
			else { this.predicateFalseEvent.Invoke(); }
		}

		public void PredicateBoolean(bool boolean)
		{
			if (boolean)
			{ this.predicateTrueEvent.Invoke(); }
			else { this.predicateFalseEvent.Invoke(); }
		}
	}
}
