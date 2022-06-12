
namespace PushForward.ScriptableObjects.Primitives
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/String", order = 3)]
    public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        public string initialValue;
        [NonSerialized] public string runtimeValue;

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            this.runtimeValue = this.initialValue;
        }
    }
}
