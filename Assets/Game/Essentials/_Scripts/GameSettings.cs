using UnityEngine;

#nullable enable
namespace DarkFrontier.Game.Essentials
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField]
        private int _targetFrameRate = 120;

        public void Initialize()
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}
#nullable restore