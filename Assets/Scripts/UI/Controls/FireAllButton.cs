using System;
using UnityEngine;
using UnityEngine.UI;

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
