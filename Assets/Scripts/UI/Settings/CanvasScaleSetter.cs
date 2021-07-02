using UnityEngine;
using UnityEngine.UI;

public class CanvasScaleSetter : MonoBehaviour {
    [SerializeField]
    private CanvasScaler _scaler;

    private void Update () {
        float scale = PlayerPrefs.GetFloat ("UIScale", 1);
        _scaler.scaleFactor = scale;
    }
}
