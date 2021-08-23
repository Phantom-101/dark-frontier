namespace DarkFrontier.Foundation.Behaviors {
    public interface IBehavior {
        void TryInitialize ();

        void Tick (float dt, float? edt = null);
        void LateTick (float dt, float? edt = null);
        void FixedTick (float dt, float? edt = null);

        bool Validate ();

        void SubscribeEventListeners ();
        void UnsubscribeEventListeners ();
    }
}
