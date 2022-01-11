using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Equipment;
using DarkFrontier.Items.Prototypes;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.UI.Equipment.Panels {
    public class HangarBayPanel : EquipmentPanel {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotRoot;
        [SerializeField] private GameObject launchablePrefab;
        [SerializeField] private Transform launchableRoot;

        private HangarBayPrototype cache;
        private readonly Dictionary<HangarLaunchableSO, HangarBayPanelLaunchable> launchables = new Dictionary<HangarLaunchableSO, HangarBayPanelLaunchable> ();

        public override void Initialize () {
            cache = Slot.Equipment as HangarBayPrototype;

            HangarBayPrototype.State state = Slot.UState as HangarBayPrototype.State;

            state.LaunchSlots.ForEach (s => {
                GameObject instantiated = Instantiate (slotPrefab, slotRoot);
                HangarBayPanelSlot comp = instantiated.GetComponent<HangarBayPanelSlot> ();
                comp.State = s;
            });
        }

        private void Update () {
            if (Slot.Equipment != cache) {
                Destroy (gameObject);
                return;
            }

            List<HangarLaunchableSO> inventoryLaunchables = Slot.Equipper.uInventory.GetStoredItems ().FindAll (i => cache.Launchables.Contains (i.uPrototype)).ConvertAll (i => (HangarLaunchableSO) i.uPrototype);
            launchables.Keys.ToList ().FindAll (k => !inventoryLaunchables.Contains (k)).ForEach (k => launchables.Remove (k));
            inventoryLaunchables.FindAll (l => !launchables.ContainsKey (l)).ForEach (l => {
                GameObject instantiated = Instantiate (launchablePrefab, launchableRoot);
                HangarBayPanelLaunchable comp = instantiated.GetComponent<HangarBayPanelLaunchable> ();
                comp.State = Slot.UState as HangarBayPrototype.State;
                comp.Launchable = l;
                launchables[l] = comp;
            });
        }
    }
}