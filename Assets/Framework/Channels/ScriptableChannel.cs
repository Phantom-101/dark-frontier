#nullable enable
using System;
using UnityEngine;

namespace Framework.Channels {
    public class ScriptableChannel<T> : ScriptableObject {
        public EventHandler<T>? onEvent;
    }
}