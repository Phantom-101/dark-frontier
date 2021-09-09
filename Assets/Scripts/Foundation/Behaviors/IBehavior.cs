namespace DarkFrontier.Foundation.Behaviors {
    public interface IBehavior {
        void TryInitialize ();
        void GetServices ();

        void Tick (float dt);
        void LateTick (float dt);
        void FixedTick (float dt);

        bool Validate ();

        void SubscribeEventListeners ();
        void UnsubscribeEventListeners ();
    }
}
