
namespace PushForward.ScriptableObjects.Primitives
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/Float", order = 2)]
    public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        public float initialValue;
        [NonSerialized] public float runtimeValue;

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            this.runtimeValue = this.initialValue;
        }
    }
}