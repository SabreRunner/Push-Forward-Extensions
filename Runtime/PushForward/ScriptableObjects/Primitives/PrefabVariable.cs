
namespace PushForward.ScriptableObjects.Primitives
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/Prefab", order = 6)]
    public class PrefabVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        public GameObject initialValue;
        [NonSerialized] public GameObject runtimeValue;

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            this.runtimeValue = this.initialValue;
        }
    }
}