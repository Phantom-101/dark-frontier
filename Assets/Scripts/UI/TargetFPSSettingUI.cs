using UnityEngine;
using UnityEngine.UI;

public class TargetFPSSettingUI : MonoBehaviour {

    [SerializeField] private VoidEventChannelSO _decreaseTargetFrameRate;
    [SerializeField] private VoidEventChannelSO _increaseTargetFrameRate;
    [SerializeField] private Text _frameRateText;
    [SerializeField] private int _targetFrameRate;

    private void Start () {

        _targetFrameRate = PlayerPrefs.HasKey ("TargetFrameRate") ? PlayerPrefs.GetInt ("TargetFrameRate") : 30;
        _frameRateText.text = _targetFrameRate.ToString ();
        Application.targetFrameRate = _targetFrameRate;

        _decreaseTargetFrameRate.OnEventRaised += () => { ChangeTargetFrameRate (-15); };
        _increaseTargetFrameRate.OnEventRaised += () => { ChangeTargetFrameRate (15); };

    }

    void ChangeTargetFrameRate (int change) {

        _targetFrameRate = Mathf.Clamp (_targetFrameRate + change, 15, 120);
        _frameRateText.text = _targetFrameRate.ToString ();
        Application.targetFrameRate = _targetFrameRate;
        PlayerPrefs.SetInt ("TargetFrameRate", _targetFrameRate);

    }

}
