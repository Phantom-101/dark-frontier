using UnityEngine;
using UnityEngine.UI;

public class ForwardSliderUI : MonoBehaviour {

    [SerializeField] private Slider _slider;

    private void Update () {

        PlayerController.GetInstance ().SetFwd (_slider.value);

    }

}
