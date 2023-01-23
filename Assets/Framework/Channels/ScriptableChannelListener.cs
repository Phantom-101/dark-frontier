using System;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Channels {
    public class ScriptableChannelListener : MonoBehaviour {
        public VoidChannel channel;
        public UnityEvent response;

        private void OnEnable() {
            channel.onEvent += OnEvent;
        }

        private void OnDisable() {
            channel.onEvent -= OnEvent;
        }

        private void OnEvent(object sender, EventArgs args) {
            response.Invoke();
        }
    }
}