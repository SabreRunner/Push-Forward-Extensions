using UnityEngine;

namespace PushForward.ScriptableObjects.Primitives
{
    using System;

    [Serializable]
    public class IntReference
    {
        public bool useInitial = true;
        public IntVariable variable;

        public int Value => this.useInitial ? this.variable.initialValue : this.variable.runtimeValue;
    }
}
