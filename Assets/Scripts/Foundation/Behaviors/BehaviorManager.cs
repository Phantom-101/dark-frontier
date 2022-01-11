using DarkFrontier.Foundation.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors
{
    public class BehaviorManager : Behavior
    {
        private readonly HashSet<IBehavior> _initializeQueue;
        private readonly HashSet<IBehavior> _enableQueue;
        private readonly HashSet<IBehavior> _disableQueue;

        public BehaviorManager() : base(false)
        {
            _initializeQueue = new HashSet<IBehavior>();
            _enableQueue = new HashSet<IBehavior>();
            _disableQueue = new HashSet<IBehavior>();
            
            InitializeImmediately(this);
            EnableImmediately(this);
        }

        public override void Enable()
        {
            Singletons.Get<BehaviorTimekeeper>().Subscribe(this);
        }

        public override void Disable()
        {
            Singletons.Get<BehaviorTimekeeper>().Unsubscribe(this);
        }

        public void InitializeImmediately(IBehavior aBehavior)
        {
            DequeueInitialize(aBehavior);
            if (aBehavior != null)
            {
                aBehavior.Initialize();
            }
        }

        public void EnableImmediately(IBehavior aBehavior)
        {
            DequeueEnable(aBehavior);
            DequeueDisable(aBehavior);
            if (aBehavior != null)
            {
                aBehavior.Enable();
            }
        }

        public void DisableImmediately(IBehavior aBehavior)
        {
            DequeueEnable(aBehavior);
            DequeueDisable(aBehavior);
            if (aBehavior != null)
            {
                aBehavior.Disable();
            }
        }

        public bool QueueInitialize(IBehavior aBehavior) => _initializeQueue.Add(aBehavior);

        public bool QueueEnable(IBehavior aBehavior)
        {
            if (!_enableQueue.Add(aBehavior)) return false;
            DequeueDisable(aBehavior);
            return true;
        }

        public bool QueueDisable(IBehavior aBehavior)
        {
            if (!_disableQueue.Add(aBehavior)) return false;
            DequeueEnable(aBehavior);
            return true;
        }

        public bool DequeueInitialize(IBehavior aBehavior) => _initializeQueue.Remove(aBehavior);
        public bool DequeueEnable(IBehavior aBehavior) => _enableQueue.Remove(aBehavior);
        public bool DequeueDisable(IBehavior aBehavior) => _disableQueue.Remove(aBehavior);

        public void AddBehavior<T, TArgs>(GameObject aGameObject, TArgs aArgs)
            where T : ComponentBehavior, ICtorArgs<TArgs>
        {
            T behavior = aGameObject.AddComponent<T>();
            behavior.Construct(aArgs);
        }

        public override void Tick(object aTicker, float aDt)
        {
            foreach (var behavior in _initializeQueue)
            {
                behavior.Initialize();
            }
            _initializeQueue.Clear();
            
            foreach (var behavior in _enableQueue)
            {
                behavior.Enable();
            }
            _enableQueue.Clear();
            
            foreach (var behavior in _disableQueue)
            {
                behavior.Disable();
            }
            _disableQueue.Clear();
        }
    }
}