/*
	FloatReference

	Description: A reference to a FloatVariable Scriptable Object.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.ScriptableObjects.Primitives
{
    using System;

    [Serializable]
    public class FloatReference
    {
		public bool useOverride = true;
		public float overrideValue;
        public bool useInitial;
        public FloatVariable variable;

        public float Value => this.useOverride ? this.overrideValue
								: this.useInitial ? this.variable.initialValue : this.variable.runtimeValue;

		public FloatReference(float value)
		{
			this.overrideValue = value;
			this.useOverride = true;
		}

		public static implicit operator float(FloatReference reference)
		{
			return reference.Value;
		}

		public static implicit operator FloatReference(float value)
		{
			return new FloatReference(value);
		}
	}
}
