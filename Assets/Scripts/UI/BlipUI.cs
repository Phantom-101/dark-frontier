using UnityEngine;
using UnityEngine.UI;

public class BlipUI : MonoBehaviour {

    [SerializeField] private Structure _target;
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _big;
    [SerializeField] private Sprite _small;
    [SerializeField] private Color _own;
    [SerializeField] private Gradient _relations;

    public Structure Target { get => _target; set => _target = value; }

    private void Update () {

        if (_target == null) {

            Destroy (gameObject);
            return;

        }

        Structure player = PlayerController.Instance.Player;
        Faction pf = player.Faction;

        if (_target == player) Destroy (gameObject);
        if (_target.Sector != player.Sector) Destroy (gameObject);

        if (_target.Profile.ApparentSize >= player.Profile.ApparentSize) _img.sprite = _big;
        else _img.sprite = _small;

        if (_target.Faction == pf) _img.color = _own;
        else {

            float r = pf.GetRelation (_target.Faction);
            _img.color = _relations.Evaluate ((r + 1) / 2);

        }

        transform.localEulerAngles = new Vector3 (0, 0, -player.GetAngleTo (_target.transform.localPosition));

    }

}
