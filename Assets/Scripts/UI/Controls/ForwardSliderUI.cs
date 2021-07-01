using UnityEngine;
using UnityEngine.UI;

public class ForwardSliderUI : MonoBehaviour {

    [SerializeField] private Slider _slider;

    private void Update () {

        PlayerController.Instance.SetFwd (_slider.value);

    }

}
