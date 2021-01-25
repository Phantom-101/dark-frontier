using UnityEngine;

public class NavigationManager : MonoBehaviour {

    private static NavigationManager _instance;

    private void Awake () {

        _instance = this;

    }

    public float GetWorldRealDistance (Structure a, Structure b) {

        return Vector3.Distance (a.transform.position, b.transform.position);

    }

    public float GetLocalRealDistance (Structure a, Structure b) {

        return Vector3.Distance (a.transform.localPosition, b.transform.localPosition);

    }

    public float GetWorldDistance (Structure a, Structure b) {

        return GetWorldRealDistance (a, b) - a.GetProfile ().ApparentSize - b.GetProfile ().ApparentSize;

    }

    public float GetLocalDistance (Structure a, Structure b) {

        return GetLocalRealDistance (a, b) - a.GetProfile ().ApparentSize - b.GetProfile ().ApparentSize;

    }

    public static NavigationManager GetInstance () { return _instance; }

}
