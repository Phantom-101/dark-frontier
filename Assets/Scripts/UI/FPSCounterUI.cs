using UnityEngine;
using UnityEngine.UI;

public class FPSCounterUI : MonoBehaviour {

    [SerializeField] private Text _text;

    private void Update () {

        _text.text = $"FPS: {(int) (1 / Time.smoothDeltaTime)}";

    }

}
