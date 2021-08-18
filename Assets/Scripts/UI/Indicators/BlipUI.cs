using DarkFrontier.Factions;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BlipUI : MonoBehaviour {
    [SerializeField] private Structure _target;
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _big;
    [SerializeField] private Sprite _small;
    [SerializeField] private Color _own;
    [SerializeField] private Gradient _relations;

    public Structure Target { get => _target; set => _target = value; }

    private FactionManager factionManager;

    [Inject]
    public void Construct (FactionManager factionManager) {
        this.factionManager = factionManager;
    }

    private void Update () {
        if (_target == null) {
            Destroy (gameObject);
            return;
        }

        Structure player = PlayerController.Instance.Player;
        string pf = player.Faction.Id.Value;

        if (_target == player) {
            Destroy (gameObject);
            return;
        }
        if (_target.Sector.Id.Value != player.Sector.Id.Value) {
            Destroy (gameObject);
            return;
        }

        if (_target.Profile.ApparentSize >= player.Profile.ApparentSize) _img.sprite = _big;
        else _img.sprite = _small;

        if (_target.Faction.Id.Value == pf) _img.color = _own;
        else {
            float r = player.Faction.Value (factionManager.GetFaction)?.GetRelation (_target.Faction.Id.Value) ?? 0;
            _img.color = _relations.Evaluate ((r + 1) / 2);
        }

        transform.localEulerAngles = new Vector3 (0, 0, -player.GetAngleTo (_target.transform.localPosition));
    }
}
