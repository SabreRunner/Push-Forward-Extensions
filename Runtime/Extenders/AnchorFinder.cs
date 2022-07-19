namespace PushForward.Extenders
{
    using UnityEngine;

    public class AnchorFinder : MonoBehaviour
    {
        public Transform anchor;
        private void OnValidate()
            => this.anchor = this.GetComponentsInChildren<Transform>()
                                 .FindFirst(go => go.name.ToLower().Contains("anchor"));
    }
}