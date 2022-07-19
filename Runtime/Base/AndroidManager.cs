/*
	AndroidManager

	Description: Useful methods and fields for handling Android.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PushForward.Base
{
    public class AndroidManager : SingletonBehaviour<AndroidManager>
    {
        #region consts
        public const string ArrayListPackageName = "java.util.ArrayList";
        public const string IntentPackageName = "android.content.Intent";
        public const string GetMethodName = "get";
        public const string SizeMethodName = "size";
        public const string ToStringMethodName = "toString";
        public const string HashCodeMethodName = "hashCode";
        public const string EqualsMethodName = "equals";
        public const string AddMethodName = "add";
        public const string SetActionMethodName = "setAction";
        public const string AddFlagsMethodName = "addFlags";
        public const string FlagReceiverForeground = "FLAG_RECEIVER_FOREGROUND";
        public const string SendBroadcastMethodName = "sendBroadcast";
        public const string ToArrayMethodName = "toArray";
        public const string IteratorMethodName = "iterator";
        public const string HasNextMethodName = "hasNext";
        public const string NextMethodName = "next";
        #endregion // consts
        
        #region general
        private static AndroidJavaObject unityPlayer;
        public static AndroidJavaObject UnityPlayer
            => unityPlayer = unityPlayer ?? new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        private static AndroidJavaObject currentActivity;
        public static AndroidJavaObject CurrentActivity
            => currentActivity = currentActivity ?? UnityPlayer?.GetStatic<AndroidJavaObject>("currentActivity");

	/// <summary>Instantiates a new Android Manager if none exist.</summary>
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

            public override string ToString()
                => this.message + "(" + this.duration + ")";

            public static ToastStructure CreateToast(string newMessage, ToastDuration newDuration = ToastDuration.Short)
                => new ToastStructure { message = newMessage, duration = newDuration};
        }
        private static AndroidJavaClass toastClass;
        private static AndroidJavaClass ToastClass
            => toastClass = toastClass ?? new AndroidJavaClass("android.widget.Toast");
        private static AndroidJavaObject toastObject;
        private static AndroidJavaObject ToastObject
            => toastObject = toastObject ?? ToastClass.CallStatic<AndroidJavaObject>("makeText", currentActivity, string.Empty, 0);
        private static readonly Queue<ToastStructure> ToastsQueue = new Queue<ToastStructure>(2);
        private static Coroutine toastCoroutine;

	/// <summary>Shows a toast according to the given toast structure.</summary>
        private static void ShowToast(ToastStructure toast)
        {
            // Instance.Log("ShowToast", "Queue Remains: " + ToastsQueue.Count + "\nShowing: " + toast);
            CurrentActivity?.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                ToastObject.Call("setText", toast.message);
                ToastObject.Call("setDuration", (int)toast.duration);
                ToastObject.Call("show");
            }));
            // Instance.Log("ShowToast", "Toasted: " + toast);
        }
        
        private static IEnumerator ShowToastsQueueCoroutine()
        {
            while (ToastsQueue.Count > 0)
            {
                ToastStructure toast = ToastsQueue.Dequeue();
                try
                { ShowToast(toast); }
                catch (Exception exception)
                { Instance.Error("ShowToastsQueueCoroutine", "Failed to send " + toast + "\n" + exception); }
                yield return new WaitForSeconds(toast.DurationInSeconds() + 0.5f);
            }

            toastCoroutine = null;
        }
        
        public static void ShowToast(string message, ToastDuration duration = ToastDuration.Short)
        {
            CheckInstance();
            
            ToastsQueue.Enqueue(ToastStructure.CreateToast(message, duration));
            if (toastCoroutine == null)
            { toastCoroutine = Instance.StartCoroutine(ShowToastsQueueCoroutine()); }
        }
        #endregion // toast

        [ContextMenu("Test")]
        public void Test()
        {
            ShowToast("Test");
        }
        
        private void Awake()
        {
            this.SetInstance(this);
        }
    }
}
#endif
