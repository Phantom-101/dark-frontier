using UnityEngine;
using UnityEngine.Events;

namespace DarkFrontier.Channels {
    [CreateAssetMenu (menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject {

        public UnityAction OnEventRaised;

        public void RaiseEvent () {

            if (OnEventRaised != null) OnEventRaised.Invoke ();

        }

    }
}
