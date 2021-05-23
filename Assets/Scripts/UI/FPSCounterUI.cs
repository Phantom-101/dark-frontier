using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounterUI : MonoBehaviour {

    [SerializeField] private Text _text;

    float deltaTime = 0.0f;

    private void Update () {

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        _text.text = $"MS: {Math.Round (msec)} / FPS: {Math.Round (fps)} / TS: {Math.Round (Time.timeScale, 2)}";

    }

}
