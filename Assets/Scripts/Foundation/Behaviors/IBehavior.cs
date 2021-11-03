namespace DarkFrontier.Foundation.Behaviors {
    public interface IBehavior {
        void Initialize ();

        void Enable ();
        void Disable ();

        void Tick (object aTicker, float aDt);
        void FixedTick (object aTicker, float aDt);
        void LateTick (object aTicker, float aDt);
    }
}
