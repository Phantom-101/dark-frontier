using DarkFrontier.Structures;
using UnityEngine;

public class NavigationManager : SingletonBase<NavigationManager> {
    public float GetWorldRealDistance (Structure a, Structure b) {
        return Vector3.Distance (a.transform.position, b.transform.position);
    }

    public float GetLocalRealDistance (Structure a, Structure b) {
        return Vector3.Distance (a.transform.localPosition, b.transform.localPosition);
    }

    public float GetWorldDistance (Structure a, Structure b) {
        return GetWorldRealDistance (a, b) - a.Profile.ApparentSize - b.Profile.ApparentSize;
    }

    public float GetLocalDistance (Structure a, Structure b) {
        return GetLocalRealDistance (a, b) - a.Profile.ApparentSize - b.Profile.ApparentSize;
    }
}
