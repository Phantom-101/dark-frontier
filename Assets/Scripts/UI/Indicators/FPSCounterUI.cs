using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Indicators {
    public class FPSCounterUI : MonoBehaviour {
        [SerializeField] private Text _text;

        private float iDeltaTime;

        private void Update () {
            iDeltaTime += (UnityEngine.Time.unscaledDeltaTime - iDeltaTime) * 0.1f;
            var lMillis = iDeltaTime * 1000.0f;
            var lFps = 1.0f / iDeltaTime;
            _text.text = $"MS: {Math.Round (lMillis).ToString(CultureInfo.InvariantCulture)} / FPS: {Math.Round (lFps).ToString(CultureInfo.InvariantCulture)} / TS: {Math.Round (UnityEngine.Time.timeScale, 2).ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
