using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangarBayPanelLaunchable : MonoBehaviour {
    public HangarBaySlotData Data;
    public HangarLaunchableSO Launchable;
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private Text quantity;

    private void Start () {
        button.onClick.AddListener (() => {
            List<HangarBayLaunchSlot> unloaded = Data.LaunchSlots.FindAll (s => s.State == HangarBayLaunchSlotState.Unloaded);
            if (unloaded.Count > 0) {
                if (Data.Slot.Equipper.Inventory.RemoveQuantity (Launchable, 1) == 1) {
                    unloaded[0].Load (Launchable);
                }
            }
        });
    }

    private void Update () {
        if (Data.Slot.Equipper == null || Launchable == null) {
            Destroy (gameObject);
            return;
        }
        int q = Data.Slot.Equipper.Inventory.GetQuantity (Launchable);
        if (q == 0) {
            Destroy (gameObject);
            return;
        }
        icon.sprite = Launchable.Icon;
        quantity.text = $"x{q}";
    }
}
