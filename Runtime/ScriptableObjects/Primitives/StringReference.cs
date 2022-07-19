
namespace PushForward.ScriptableObjects.Primitives
{
    using System;

    [Serializable]
    public class StringReference
    {
        public bool useOverride = true;
        public string overrideValue;
        public bool useInitial;
        public StringVariable variable;

        public event Action<string> Updated;
        public string Value
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
