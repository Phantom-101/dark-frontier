using DarkFrontier.Foundation.Services;
using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors
{
    public class ComponentBehavior : MonoBehaviour, IBehavior
    {
        public bool Enabled { get; set; }

        protected BehaviorManager oBehaviorManager;

        protected virtual void Awake()
        {
            oBehaviorManager = Singletons.Get<BehaviorManager>();
            oBehaviorManager.QueueInitialize(this);
        }

        protected virtual void OnEnable()
        {
            oBehaviorManager.QueueEnable(this);
        }

        protected virtual void OnDisable()
        {
            oBehaviorManager.QueueDisable(this);
        }

        protected virtual void OnDestroy()
        {
            oBehaviorManager.DisableImmediately(this);
        }

        public virtual void Initialize()
        {
        }

        public virtual void Enable()
        {
            Enabled = true;
        }

        public virtual void Disable()
        {
            Enabled = false;
        }

        public virtual void Tick(object aTicker, float aDt)
        {
        }

        public virtual void FixedTick(object aTicker, float aDt)
        {
        }

        public virtual void LateTick(object aTicker, float aDt)
        {
        }
    }
}