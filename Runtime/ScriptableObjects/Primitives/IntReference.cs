/*
	IntReference

	Description: A reference to an IntVariable Scriptable Object

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.ScriptableObjects.Primitives
{
    using System;

    [Serializable]
    public class IntReference
    {
		public bool useOverride = true;
		public int overrideValue;
		public bool useInitial;
        public IntVariable variable;

		public event Action<int> Updated;
		public int Value
		{
			get => this.useOverride ? this.overrideValue
				   : this.useInitial ? this.variable.initialValue : this.variable.runtimeValue;
			set
			{
				if (this.useOverride == false)
				{
					this.useInitial = false;
					this.variable.runtimeValue = value;
					this.Updated?.Invoke(this.variable.runtimeValue);
				}
			}
		}
    }
}
