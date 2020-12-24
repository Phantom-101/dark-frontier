using UnityEngine;
using UnityEngine.Events;

public class LoadEventChannelSO : ScriptableObject {

    public UnityAction<GameSceneSO[], bool> OnLoadingRequested;

    public void RaiseEvent (GameSceneSO[] locationsToLoad, bool showLoadingScreen) {

        if (OnLoadingRequested != null) OnLoadingRequested.Invoke (locationsToLoad, showLoadingScreen);

    }

}
