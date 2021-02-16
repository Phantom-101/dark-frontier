using UnityEngine;

public class TargetUI : MonoBehaviour {

    [SerializeField] private Transform _canvas;
    [SerializeField] private GameObject _infoPanelPrefab;
    [SerializeField] private VoidEventChannelSO _targetChangedChannel;

    private GameObject _instantiated;

    private void Awake () {

        _targetChangedChannel.OnEventRaised += UpdateUI;

    }

    private void UpdateUI () {

        if (_instantiated != null) Destroy (_instantiated);

        _instantiated = Instantiate (_infoPanelPrefab, _canvas);
        _instantiated.GetComponent<TargetInfoUI> ().SetStructure (PlayerController.GetInstance ().GetPlayer ().Target);

    }

}
