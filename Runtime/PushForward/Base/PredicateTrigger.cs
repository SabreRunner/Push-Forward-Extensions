namespace PushForward
{
	using UnityEngine;
	using UnityEngine.Events;

	public class PredicateTrigger : BaseMonoBehaviour
	{
		[SerializeField] private UnityEvent predicateTrueEvent;
		[SerializeField] private UnityEvent predicateFalseEvent;

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
