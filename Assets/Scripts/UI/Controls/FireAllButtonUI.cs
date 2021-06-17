using System;

public class FireAllButtonUI : SingletonBase<FireAllButtonUI> {
    public EventHandler FireAll;

    public void PointerDown () {
        FireAll?.Invoke (this, EventArgs.Empty);
    }
}
