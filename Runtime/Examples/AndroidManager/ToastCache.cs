
namespace PushForward.Examples.AndroidManager
{
    using PushForward;

    public class ToastCache : BaseMonoBehaviour
    {
        public string StringValue { get; set; }

        public void SendToast()
        {
            #if UNITY_ANDROID
            AndroidManager.ShowToast(this.StringValue);
            #endif
        }
    }
}
