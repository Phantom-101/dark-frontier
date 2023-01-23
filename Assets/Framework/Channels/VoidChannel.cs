using System;
using UnityEngine;

namespace Framework.Channels {
    [CreateAssetMenu(menuName = "Channel/Void", fileName = "NewVoidChannel")]
    public class VoidChannel : ScriptableChannel<EventArgs> { }
}