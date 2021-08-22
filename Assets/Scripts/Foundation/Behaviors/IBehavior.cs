namespace DarkFrontier.Foundation.Behaviors {
    public interface IBehavior {
        void TryInitialize ();

        void Tick (float dt, float? edt);
        void LateTick (float dt, float? edt);
        void FixedTick (float dt, float? edt);

        bool Validate ();

        void SubscribeEventListeners ();
        void UnsubscribeEventListeners ();
    }
}
