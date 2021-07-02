using System;
using UnityEngine;
using UnityEngine.UI;

public class FireAllButton : SingletonBase<FireAllButton> {
    public EventHandler FireAll;
    [SerializeField]
    private Button _fireAllButton;

    private void Awake () {
        // Set up button listener
        _fireAllButton.onClick.AddListener (FireAllButtonPressed);
    }

    public void FireAllButtonPressed () {
        // Invoke event
        FireAll?.Invoke (this, EventArgs.Empty);
    }
}
