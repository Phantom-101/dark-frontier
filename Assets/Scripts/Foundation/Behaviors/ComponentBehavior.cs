using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class ComponentBehavior : MonoBehaviour, IBehavior {
        [SerializeField] protected bool initialized;
        [SerializeField] protected bool subscribed;
        public bool CanTickSelf { get => canTickSelf; set => canTickSelf = value; }
        [SerializeField] protected bool canTickSelf = true;

        private void Update () {
            if (canTickSelf) Tick (Time.deltaTime);
        }

        private void LateUpdate () {
            if (canTickSelf) LateTick (Time.deltaTime);
        }

        private void FixedUpdate () {
            if (canTickSelf) FixedTick (Time.fixedDeltaTime);
        }

        private void OnEnable () {
            TryInitialize ();
            SubscribeEventListeners ();
        }

        private void OnDisable () {
            UnsubscribeEventListeners ();
        }

        /// <summary>
        /// Tries to initialize the behavior.
        /// Service getting and single-initialization will always execute before multi-initialization.
        /// If an exception occurs while getting services or single-initializing, this behavior will not be marked as initialized.
        /// This function is called by default in the OnEnable event function.
        /// If you need this behavior to be initialized as soon as it is created, call this function manually.
        /// Service getting occurs before single-initialization and both will occur only once once the initialized flag is set.
        /// </summary>
        public void TryInitialize () {
            if (!initialized) {
                GetServices ();
                SingleInitialize ();
                initialized = true;
            }
            MultiInitialize ();
        }

        /// <summary>
        /// Initialization code that is meant to be only run once.
        /// </summary>
        protected virtual void SingleInitialize () { }
        /// <summary>
        /// Initialization code that can be run multiple times without unintended effects.
        /// </summary>
        protected virtual void MultiInitialize () { }
        /// <summary>
        /// Initializes all service references
        /// </summary>
        public virtual void GetServices () { }

        /// <summary>
        /// Ticks the behavior according to the game loop. Equivalent to the Update event function.
        /// </summary>
        /// <param name="dt">Time since last tick.</param>
        public void Tick (float dt) {
            if (!Validate ()) {
                ValidateFailed ();
                return;
            }
            InternalTick (dt);
            PropagateTick (dt);
        }

        protected virtual void InternalTick (float dt) { }
        protected virtual void PropagateTick (float dt) { }

        /// <summary>
        /// Ticks the behavior after the game loop has elapsed. Equivalent to the LateUpdate event function.
        /// </summary>
        /// <param name="dt">Time since last tick.</param>
        public void LateTick (float dt) {
            if (!Validate ()) {
                ValidateFailed ();
                return;
            }
            InternalLateTick (dt);
            PropagateLateTick (dt);
        }

        protected virtual void InternalLateTick (float dt) { }
        protected virtual void PropagateLateTick (float dt) { }

        /// <summary>
        /// Ticks the behavior according to the physics loop. Equivalent to the FixedUpdate event function.
        /// </summary>
        /// <param name="dt">Time since last tick.</param>
        public void FixedTick (float dt) {
            if (!Validate ()) {
                ValidateFailed ();
                return;
            }
            InternalFixedTick (dt);
            PropagateFixedTick (dt);
        }

        protected virtual void InternalFixedTick (float dt) { }
        protected virtual void PropagateFixedTick (float dt) { }

        /// <summary>
        /// Attempts to validate all of the variables of this behavior.
        /// </summary>
        /// <returns>Whether or not potential issues were successfully resolved.</returns>
        public virtual bool Validate () => true;
        protected virtual void ValidateFailed () { }

        public void SubscribeEventListeners () {
            if (!subscribed) {
                InternalSubscribeEventListeners ();
                subscribed = true;
            }
        }

        protected virtual void InternalSubscribeEventListeners () { }

        public void UnsubscribeEventListeners () {
            if (subscribed) {
                InternalUnsubscribeEventListeners ();
                subscribed = false;
            }
        }

        protected virtual void InternalUnsubscribeEventListeners () { }
    }
}