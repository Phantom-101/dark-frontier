using UnityEngine;

namespace Framework.Channels {
    [CreateAssetMenu(menuName = "Channel/Float", fileName = "NewFloatChannel")]
    public class FloatChannel : ScriptableChannel<float> { }
}