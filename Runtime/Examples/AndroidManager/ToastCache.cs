
#if UNITY_ANDROID
namespace PushForward.Examples
{
    using Base;
    using UnityEngine;

    /// <summary>An example component used to send the given string value as an Android Toast.</summary>
    public class ToastCache : MonoBehaviour
    {
        /// <summary>The string to send as Toast.</summary>
        public string StringValue { get; set; }

        /// <summary>An editor exposed method that sends the set string value as an Android Toast.</summary>
        public void SendToast()
        {
            AndroidManager.ShowToast(this.StringValue);
        }
    }
}
#endif
