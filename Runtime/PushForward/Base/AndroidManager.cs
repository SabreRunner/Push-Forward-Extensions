using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PushForward
{
    public class AndroidManager : SingletonBehaviour<AndroidManager>
    {
        #if UNITY_ANDROID
        #region general
        private static AndroidJavaObject unityPlayer;
        public static AndroidJavaObject UnityPlayer => unityPlayer = unityPlayer ?? new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        private static AndroidJavaObject unityActivity;
        public static AndroidJavaObject UnityActivity => unityActivity = unityActivity ?? UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        private static void CheckInstance()
        {
            if (Instance != null) { return; }

            GameObject gameObject = new GameObject("Android Manager");
            gameObject.AddComponent<AndroidManager>();
        }
        #endregion // general

        #region toast
        public enum ToastDuration { Short, Long }
        private struct ToastStructure
        {
            public string message;
            public ToastDuration duration;

            public float DurationInSeconds()
                => this.duration == ToastDuration.Short ? 2f : 3.5f;
            public static ToastStructure CreateToast(string newMessage, ToastDuration newDuration = ToastDuration.Short)
                => new ToastStructure { message = newMessage, duration = newDuration};
        }
        private static AndroidJavaClass toastClass;
        private static AndroidJavaClass ToastClass => toastClass = toastClass ?? new AndroidJavaClass("android.widget.Toast");
        private static AndroidJavaObject toastObject;
        private static AndroidJavaObject ToastObject => toastObject = toastObject ?? ToastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, string.Empty, 0);
        private static readonly Queue<ToastStructure> ToastsQueue = new Queue<ToastStructure>(2);
        private static Coroutine toastCoroutine;

        private static void ShowToast(ToastStructure toast)
        {
            // Instance.Log("ShowToast", "Showing " + toast.message + " for a " + toast.duration + " time.");
            UnityActivity?.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                ToastObject.Call("setText", toast.message);
                ToastObject.Call("setDuration", (int)toast.duration);
                ToastObject.Call("show");
            }));
        }

        private static IEnumerator ShowToastsQueueCoroutine()
        {
            while (ToastsQueue.Count > 0)
            {
                ToastStructure toast = ToastsQueue.Dequeue();
                ShowToast(toast);
                yield return new WaitForSeconds(toast.DurationInSeconds());
            }

            AndroidManager.toastCoroutine = null;
        }

        public static void ShowToast(string message, ToastDuration duration = ToastDuration.Short)
        {
            CheckInstance();

            ToastsQueue.Enqueue(ToastStructure.CreateToast(message, duration));
            if (toastCoroutine == null)
            { toastCoroutine = Instance.StartCoroutine(ShowToastsQueueCoroutine()); }
        }
        #endregion // toast
        #endif

        private void Awake()
        {
            this.SetInstance(this);
        }
    }
}
