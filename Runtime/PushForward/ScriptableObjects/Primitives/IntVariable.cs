
namespace PushForward.ScriptableObjects.Primitives
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/Int", order = 1)]
    public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        public int initialValue;
        [NonSerialized] public int runtimeValue;

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            this.runtimeValue = this.initialValue;
        }
    }
}
