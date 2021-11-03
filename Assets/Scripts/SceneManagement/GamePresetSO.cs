using UnityEngine;

namespace DarkFrontier.SceneManagement {
    [CreateAssetMenu (menuName = "Game Preset")]
    public class GamePresetSO : ScriptableObject {

        public string PresetName;
        public string Description;
        public string SceneName;

    }
}
