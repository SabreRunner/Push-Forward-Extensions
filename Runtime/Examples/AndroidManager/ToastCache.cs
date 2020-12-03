
namespace Examples.AndroidManager
{
    using PushForward;

    public class ToastCache : BaseMonoBehaviour
    {
        public string StringValue { get; set; }

        public void SendToast()
        {
            AndroidManager.ShowToast(this.StringValue);
        }
    }
}
