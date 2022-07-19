
namespace PushForward.ScriptableObjects.Primitives
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PrefabReference
    {
        public bool useOverride = true;
        public GameObject overrideValue;
        public bool useInitial;
        public PrefabVariable variable;

        public event Action<GameObject> Updated;
        public GameObject Value
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