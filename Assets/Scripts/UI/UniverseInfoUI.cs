using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI {
    public class UniverseInfoUI : MonoBehaviour {
        [SerializeField] private Button _button;
        public Button Button {
            get { return _button; }
        }
        [SerializeField] private Text _nameText;
        public string Name {
            set { _nameText.text = value; }
        }
    }
}
