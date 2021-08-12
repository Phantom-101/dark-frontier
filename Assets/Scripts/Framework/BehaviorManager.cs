using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BehaviorManager : MonoBehaviour {
    [SerializeField] protected float tickInterval;
    protected float tickCounter;
    [SerializeField] protected int expensiveTickPer;
    protected int ticksPassed;
    protected float expensiveTickCounter;
    [SerializeField] protected float lateTickInterval;
    protected float lateTickCounter;
    [SerializeField] protected int expensiveLateTickPer;
    protected int lateTicksPassed;
    protected float expensiveLateTickCounter;
    [SerializeField] protected float fixedTickInterval;
    protected float fixedTickCounter;
    [SerializeField] protected int expensiveFixedTickPer;
    protected int fixedTicksPassed;
    protected float expensiveFixedTickCounter;

    protected readonly HashSet<BehaviorBase> managed = new HashSet<BehaviorBase> ();
    public List<BehaviorBase> Managed { get => managedList ?? (managedList = managed.ToList ()); }
    [SerializeField] protected List<BehaviorBase> managedList;

    private void Update () {
        tickCounter += Time.deltaTime;
        expensiveTickCounter += Time.deltaTime;

        if (tickCounter >= tickInterval) {
            ticksPassed++;
            if (ticksPassed >= expensiveTickPer) {
                Managed.ForEach (e => e.Tick (tickCounter, expensiveTickCounter));
                ticksPassed = 0;
                expensiveTickCounter = 0;
            } else {
                Managed.ForEach (e => e.Tick (tickCounter));
            }
            tickCounter = 0;
        }
    }

    private void LateUpdate () {
        lateTickCounter += Time.deltaTime;
        expensiveLateTickCounter += Time.deltaTime;

        if (lateTickCounter >= lateTickInterval) {
            lateTicksPassed++;
            if (lateTicksPassed >= expensiveLateTickPer) {
                Managed.ForEach (e => e.LateTick (lateTickCounter, expensiveLateTickCounter));
                lateTicksPassed = 0;
                expensiveLateTickCounter = 0;
            } else {
                Managed.ForEach (e => e.LateTick (lateTickCounter));
            }
            lateTickCounter = 0;
        }
    }

    private void FixedUpdate () {
        fixedTickCounter += Time.fixedDeltaTime;
        expensiveFixedTickCounter += Time.fixedDeltaTime;

        if (fixedTickCounter >= fixedTickInterval) {
            fixedTicksPassed++;
            if (fixedTicksPassed >= expensiveFixedTickPer) {
                Managed.ForEach (e => e.FixedTick (fixedTickCounter, expensiveFixedTickCounter));
                fixedTicksPassed = 0;
                expensiveFixedTickCounter = 0;
            } else {
                Managed.ForEach (e => e.FixedTick (fixedTickCounter));
            }
            fixedTickCounter = 0;
        }
    }

    public virtual bool TryManage (BehaviorBase behavior) {
        if (behavior.Manager != null || behavior.Manager == this) return false;
        managed.Add (behavior);
        managedList = null;
        behavior.Manager = this;
        return true;
    }

    public virtual void ForceManage (BehaviorBase behavior) {
        if (behavior.Manager == this) return;
        if (behavior.Manager != null) {
            behavior.Manager.RemoveManage (behavior);
            behavior.Manager = null;
        }
        managed.Add (behavior);
        managedList = null;
        behavior.Manager = this;
    }

    public virtual bool RemoveManage (BehaviorBase behavior) {
        if (managed.Remove (behavior)) {
            managedList = null;
            behavior.Manager = null;
            return true;
        }
        return false;
    }
}
