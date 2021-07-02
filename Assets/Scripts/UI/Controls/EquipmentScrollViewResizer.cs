using UnityEngine;

public class EquipmentScrollViewResizer : MonoBehaviour {
    [SerializeField]
    private RectTransform _transform;

    private void Update () {
        // Resize equipment buttons SV to fit between the selected info panel and equipment control indicators
        _transform.sizeDelta = new Vector2 (_transform.sizeDelta.x, Screen.height - 585);
    }
}
