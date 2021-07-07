using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour {
    [SerializeField] private Transform _hpIndicators;
    [SerializeField] private Image _hull;
    [SerializeField] private Gradient _hullGradient;
    [SerializeField] private Image _shield;
    [SerializeField] private Gradient _shieldGradient;
    [SerializeField] private Transform _direction;
    [SerializeField] private RectTransform _capFill;
    [SerializeField] private Image _capImg;
    [SerializeField] private RectTransform _capOutline;
    [SerializeField] private Gradient _capGradient;

    private void Update () {
        Structure player = PlayerController.Instance.Player;

        if (player == null) return;

        // Hull wireframe indicator
        _hull.sprite = player.Profile.HullWireframe;
        _hull.color = _hullGradient.Evaluate (player.Hull / player.Stats.GetBaseValue (StatNames.MaxHull, 1));

        // Shield bubble indicator
        float curStrength = 0, totalStrength = 0;
        player.GetEquipmentData<ShieldSlotData> ().ForEach (shield => {
            curStrength += shield.Strength;
            totalStrength += (shield.Equipment as ShieldSO).MaxStrength;
        });
        _shield.color = _shieldGradient.Evaluate (curStrength / (totalStrength == 0 ? 1 : totalStrength));

        // Velocity indicator
        //Rigidbody rb = _structure.GetComponent<Rigidbody> ();
        //if (rb == null) _velocity.text = "0 m/s";
        //else _velocity.text = rb.velocity.magnitude.ToString ("F2") + " m/s";

        // Capacitor bar indicator
        float storedCap = 0, totalCap = 0;
        player.GetEquipmentData<CapacitorSlotData> ().ForEach (capacitor => {
            storedCap += capacitor.Charge;
            totalCap += (capacitor.Equipment as CapacitorSO).Capacitance;
        });
        _capFill.sizeDelta = new Vector2 (_capOutline.sizeDelta.x * storedCap / (totalCap == 0 ? 1 : totalCap), _capFill.sizeDelta.y);
        _capImg.color = _capGradient.Evaluate (storedCap / (totalCap == 0 ? 1 : totalCap));

        // Selected direction indicator
        if (player.Selected == null) _direction.gameObject.SetActive (false);
        else {
            _direction.gameObject.SetActive (true);
            _direction.rotation = Quaternion.Euler (0, 0, -player.GetAngleTo (player.Selected.transform.position));
        }
    }
}
