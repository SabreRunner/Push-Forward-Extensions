
namespace PushForward.ScriptableObjects.Primitives
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PrefabReference
    {
        public bool useInitial = true;
        public PrefabVariable variable;

        public GameObject Value => this.useInitial ? this.variable.initialValue : this.variable.runtimeValue;
    }
}