using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;

public class LockedTargetUI : ComponentBehavior {
    public Structure Structure;

    [SerializeField] protected Button button;
    [SerializeField] protected Image hull;
    [SerializeField] protected Image shield;
    [SerializeField] protected Image leftLock;
    [SerializeField] protected Image rightLock;
    [SerializeField] protected Text nameText;
    [SerializeField] protected Text factionText;
    [SerializeField] protected Text distanceText;
    [SerializeField] protected Text velocityText;
    [SerializeField] protected Transform direction;

    private PlayerController playerController;

    protected override void SingleInitialize () {
        playerController = PlayerController.Instance;
    }

    protected override void InternalSubscribeEventListeners () {
        button.onClick.AddListener (() => { playerController.Player.Selected = Structure; });
    }

    protected override void InternalTick (float dt) {
        if (Structure == null) {
            Destroy (gameObject);
            return;
        }

        Structure player = playerController.Player;

        if (!player.Locks.ContainsKey (Structure)) {
            Destroy (gameObject);
            return;
        }

        hull.fillAmount = Structure.Hull / Structure.Stats.GetBaseValue (StatNames.MaxHull, 1) / 2;
        float strength = 0, maxStrength = 0;
        Structure.GetEquipmentStates<ShieldPrototype.State> ().ForEach (shield => {
            strength += shield.Strength;
            maxStrength += (shield.Slot.Equipment as ShieldPrototype).MaxStrength;
        });
        shield.fillAmount = strength / (maxStrength == 0 ? 1 : maxStrength) / 2;
        float fa = player.Locks[Structure] / 400;
        leftLock.fillAmount = fa;
        rightLock.fillAmount = fa;
        nameText.text = Structure.gameObject.name;
        factionText.text = Structure.Faction.Value?.Name ?? "None";
        distanceText.text = Vector3.Distance (PlayerController.Instance.Player.transform.position, Structure.transform.position).ToString ("F0") + " m";
        Rigidbody rb = Structure.GetComponent<Rigidbody> ();
        if (rb == null) velocityText.text = "0 m/s";
        else velocityText.text = rb.velocity.magnitude.ToString ("F0") + " m/s";
        direction.rotation = Quaternion.Euler (0, 0, -Structure.GetAngleTo (PlayerController.Instance.Player.transform.position));
    }
}
