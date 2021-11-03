using System;
using DarkFrontier.Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Controls {
    public class FireAllButton : SingletonBase<FireAllButton> {
        public event EventHandler OnClicked;
        [SerializeField]
        private Button _fireAllButton;

        private void Awake () {
            // Set up button listener
            _fireAllButton.onClick.AddListener (FireAllButtonPressed);
        }

        public void FireAllButtonPressed () {
            // Invoke event
            OnClicked?.Invoke (this, EventArgs.Empty);
        }
    }
}
