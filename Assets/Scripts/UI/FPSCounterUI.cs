using UnityEngine;
using UnityEngine.UI;

public class FPSCounterUI : MonoBehaviour {

    [SerializeField] private Text _text;

    private void Update () {

        if (Time.timeScale == 0) return;

        _text.text = $"FPS: {(int) (Time.timeScale / Time.smoothDeltaTime)}";

    }

}
