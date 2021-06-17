public class ReverseButtonUI : SingletonBase<ReverseButtonUI> {
    public bool Reversing;

    public void PointerDown () {
        Reversing = true;
    }

    public void PointerUp () {
        Reversing = false;
    }
}