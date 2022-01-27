using UnityEngine;


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
