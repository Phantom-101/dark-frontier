using UnityEngine;

public class SelectedUI : MonoBehaviour {

    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _infoPanelPrefab;
    [SerializeField] private VoidEventChannelSO _selectedChangedChannel;

    private GameObject _instantiated;

    private void Awake () {

        _selectedChangedChannel.OnEventRaised += UpdateUI;

    }

    private void UpdateUI () {

        if (_instantiated != null) Destroy (_instantiated);

        _instantiated = Instantiate (_infoPanelPrefab, _transform);
        _instantiated.GetComponent<SelectedInfoUI> ().SetStructure (PlayerController.GetInstance ().GetPlayer ().Selected);

    }

}
