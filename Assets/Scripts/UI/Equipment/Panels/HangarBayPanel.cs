using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HangarBayPanel : EquipmentPanel {
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotRoot;
    [SerializeField] private GameObject launchablePrefab;
    [SerializeField] private Transform launchableRoot;

    private HangarBaySO cache;
    private readonly Dictionary<HangarLaunchableSO, HangarBayPanelLaunchable> launchables = new Dictionary<HangarLaunchableSO, HangarBayPanelLaunchable> ();

    public override void Initialize () {
        cache = Slot.Data.Equipment as HangarBaySO;

        HangarBaySlotData data = Slot.Data as HangarBaySlotData;

        data.LaunchSlots.ForEach (s => {
            GameObject instantiated = Instantiate (slotPrefab, slotRoot);
            HangarBayPanelSlot comp = instantiated.GetComponent<HangarBayPanelSlot> ();
            comp.LaunchSlot = s;
        });
    }

    private void Update () {
        if (Slot.Data.Equipment != cache) {
            Destroy (gameObject);
            return;
        }

        List<HangarLaunchableSO> inventoryLaunchables = Slot.Equipper.Inventory.GetStoredItems ().FindAll (i => cache.Launchables.Contains (i)).ConvertAll (i => i as HangarLaunchableSO);
        launchables.Keys.ToList ().FindAll (k => !inventoryLaunchables.Contains (k)).ForEach (k => launchables.Remove (k));
        inventoryLaunchables.FindAll (l => !launchables.ContainsKey (l)).ForEach (l => {
            GameObject instantiated = Instantiate (launchablePrefab, launchableRoot);
            HangarBayPanelLaunchable comp = instantiated.GetComponent<HangarBayPanelLaunchable> ();
            comp.Data = Slot.Data as HangarBaySlotData;
            comp.Launchable = l;
            launchables[l] = comp;
        });
    }
}
