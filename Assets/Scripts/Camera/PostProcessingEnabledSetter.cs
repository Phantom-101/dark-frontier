using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PostProcessingEnabledSetter : MonoBehaviour {

    [SerializeField] private VoidEventChannelSO _postProcessingEnabledToggled;
    [SerializeField] private UniversalAdditionalCameraData _cameraData;

    private void Awake () {

        //SetPostProcessingEnabled ();

        //_postProcessingEnabledToggled.OnEventRaised += SetPostProcessingEnabled;

    }

    private void Update () {

        SetPostProcessingEnabled ();

    }

    private void SetPostProcessingEnabled () {

        _cameraData.renderPostProcessing = PlayerPrefs.GetInt ("PostProcessingEnabled") == 1;

    }

}
