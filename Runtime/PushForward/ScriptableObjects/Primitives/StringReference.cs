
namespace PushForward.ScriptableObjects.Primitives
{
    using System;

    [Serializable]
    public class StringReference
    {
        public bool useInitial = true;
        public StringVariable variable;

        public string Value => this.useInitial ? this.variable.initialValue : this.variable.runtimeValue;
    }
}
