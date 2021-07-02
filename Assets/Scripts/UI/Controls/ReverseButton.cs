public class ReverseButton : SingletonBase<ReverseButton> {
    public bool Reversing { get; private set; }

    public void PointerDown () {
        Reversing = true;
    }

    public void PointerUp () {
        Reversing = false;
    }
}