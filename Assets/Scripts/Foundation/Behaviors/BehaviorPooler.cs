using System;
using System.Collections;
using System.Collections.Generic;
using DarkFrontier.Foundation.Extensions;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Foundation.Behaviors {
    public class BehaviorPooler : MonoBehaviour {
        private readonly Dictionary<object, List<IBehavior>> iPools = new Dictionary<object, List<IBehavior>>();

        public IBehavior Take(object aKey, Func<IBehavior> aBuilder) {
            iPools.TryAdd(aKey, new List<IBehavior>());
            List<IBehavior> lBehaviors = iPools[aKey];
            var lCount = lBehaviors.Count;
            if (lCount == 0) {
                return aBuilder.Invoke();
            }

            IBehavior lRet = lBehaviors[lCount - 1];
            lBehaviors.RemoveAt(lCount - 1);
            return lRet;
        }

        public void Reclaim(object aKey, IBehavior aBehavior) {
            iPools.TryAdd(aKey, new List<IBehavior>());
            iPools[aKey].Add(aBehavior);
        }

        public void ReclaimAfter(object aKey, IBehavior aBehavior, float aTime) {
            StartCoroutine(InternalReclaimAfter(aKey, aBehavior, aTime));
        }

        private IEnumerator InternalReclaimAfter(object aKey, IBehavior aBehavior, float aTime) {
            yield return new WaitForSeconds(aTime);
            Reclaim(aKey, aBehavior);
        }
    }
}
#nullable restore
