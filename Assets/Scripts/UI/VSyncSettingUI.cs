using UnityEngine;
using UnityEngine.UI;

public class VSyncSettingUI : MonoBehaviour {

    [SerializeField] private VoidEventChannelSO _decreaseVSyncCount;
    [SerializeField] private VoidEventChannelSO _increaseVSyncCount;
    [SerializeField] private Text _vSyncText;
    [SerializeField] private int _vSyncCount;

    private void Start () {

        _vSyncCount = PlayerPrefs.HasKey ("VSyncCount") ? PlayerPrefs.GetInt ("VSyncCount") : 1;
        _vSyncText.text = _vSyncCount.ToString ();
        QualitySettings.vSyncCount = _vSyncCount;

        _decreaseVSyncCount.OnEventRaised += () => { ChangeVSyncCount (-1); };
        _increaseVSyncCount.OnEventRaised += () => { ChangeVSyncCount (1); };

    }

    void ChangeVSyncCount (int change) {

        _vSyncCount = Mathf.Clamp (_vSyncCount + change, 0, 4);
        _vSyncText.text = _vSyncCount.ToString ();
        QualitySettings.vSyncCount = _vSyncCount;
        PlayerPrefs.SetInt ("VSyncCount", _vSyncCount);

    }

}
