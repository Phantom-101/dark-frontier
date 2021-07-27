using UnityEngine;

public class BehaviorBase : MonoBehaviour {
    [SerializeField] protected bool initialized;
    public BehaviorManager Manager { get => manager; set => manager = value; }
    [SerializeField] protected BehaviorManager manager;

    private void Start () {
        TryInitialize ();
    }

    private void Update () {
        if (manager == null) Tick (Time.deltaTime, Time.deltaTime);
    }

    private void LateUpdate () {
        if (manager == null) LateTick (Time.deltaTime, Time.deltaTime);
    }

    private void FixedUpdate () {
        if (manager == null) FixedTick (Time.fixedDeltaTime, Time.fixedDeltaTime);
    }

    private void OnEnable () {
        SubscribeEventListeners ();
    }

    private void OnDisable () {
        UnsubscribeEventListeners ();
    }

    /// <summary>
    /// Tries to initialize the behavior.
    /// Single-initialization will always execute before multi-initialization.
    /// If an exception occurs while single-initializing, this behavior will not be marked as initialized.
    /// This function is called by default in the Start event function.
    /// If you need this behavior to be initialized as soon as it is created, call this function manually.
    /// </summary>
    public void TryInitialize () {
        if (!initialized) {
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
    /// Ticks the behavior according to the game loop. Equivalent to the Update event function.
    /// Expensive operations will always execute after non-expensive operations have finished running.
    /// </summary>
    /// <param name="dt">Time since last non-expensive tick.</param>
    /// <param name="edt">Time since last expensive tick. If null, expensive operations will not be run.</param>
    public void Tick (float dt, float? edt = null) {
        if (!Validate ()) {
            ValidateFailed ();
            return;
        }
        InternalTick (dt);
        if (edt != null) InternalExpensiveTick (edt.Value);
    }

    protected virtual void InternalTick (float dt) { }
    protected virtual void InternalExpensiveTick (float dt) { }

    /// <summary>
    /// Ticks the behavior after the game loop has elapsed. Equivalent to the LateUpdate event function.
    /// Expensive operations will always execute after non-expensive operations have finished running.
    /// </summary>
    /// <param name="dt">Time since last non-expensive tick.</param>
    /// <param name="edt">Time since last expensive tick. If null, expensive operations will not be run.</param>
    public void LateTick (float dt, float? edt = null) {
        if (!Validate ()) {
            ValidateFailed ();
            return;
        }
        InternalLateTick (dt);
        if (edt != null) InternalExpensiveLateTick (edt.Value);
    }

    protected virtual void InternalLateTick (float dt) { }
    protected virtual void InternalExpensiveLateTick (float dt) { }

    /// <summary>
    /// Ticks the behavior according to the physics loop. Equivalent to the FixedUpdate event function.
    /// Expensive operations will always execute after non-expensive operations have finished running.
    /// </summary>
    /// <param name="dt">Time since last non-expensive tick.</param>
    /// <param name="edt">Time since last expensive tick. If null, expensive operations will not be run.</param>
    public void FixedTick (float dt, float? edt = null) {
        if (!Validate ()) {
            ValidateFailed ();
            return;
        }
        InternalFixedTick (dt);
        if (edt != null) InternalExpensiveFixedTick (edt.Value);
    }

    protected virtual void InternalFixedTick (float dt) { }
    protected virtual void InternalExpensiveFixedTick (float dt) { }

    /// <summary>
    /// Attempts to validate all of the variables of this behavior.
    /// </summary>
    /// <returns>Whether or not potential issues were successfully resolved.</returns>
    public virtual bool Validate () => true;
    protected virtual void ValidateFailed () { }

    protected virtual void SubscribeEventListeners () { }
    protected virtual void UnsubscribeEventListeners () { }
}
