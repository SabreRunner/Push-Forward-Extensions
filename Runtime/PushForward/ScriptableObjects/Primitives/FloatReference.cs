
namespace PushForward.ScriptableObjects.Primitives
{
    using System;

    [Serializable]
    public class FloatReference
    {
		public bool useOverride = true;
		public float overrideValue = 0f;
        public bool useInitial = false;
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
