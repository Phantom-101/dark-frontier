using DarkFrontier.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Equipment.Panels {
    public class HangarBayPanelSlot : MonoBehaviour {
        public HangarBayPrototype.State.SlotState State;
        [SerializeField] private Button button;
        [SerializeField] private Image fill;
        [SerializeField] private Image indicator;
        [SerializeField] private Image icon;
        [SerializeField] private Text status;

        private Color orange = new Color (1, 0.5f, 0);
        private Color aqua = new Color (0, 1, 1);

        private void Start () {
            button.onClick.AddListener (() => {
                HangarBayPrototype.State.SlotState.SlotStatus status = State.Status;
                if (status == HangarBayPrototype.State.SlotState.SlotStatus.Loading || status == HangarBayPrototype.State.SlotState.SlotStatus.Loaded) State.Status = HangarBayPrototype.State.SlotState.SlotStatus.Unloading;
            });
        }

        private void Update () {
            HangarBayPrototype.State.SlotState.SlotStatus state = State.Status;
            if (state == HangarBayPrototype.State.SlotState.SlotStatus.Unloaded) {
                fill.fillAmount = 0;
                indicator.color = Color.red;
                icon.sprite = null;
                status.text = "Empty";
            } else if (state == HangarBayPrototype.State.SlotState.SlotStatus.Unloading) {
                fill.fillAmount = State.LoadingProgress / State.Launchable.LoadingPreparation;
                indicator.color = orange;
                icon.sprite = State.Launchable.Icon;
                status.text = "Unloading";
            } else if (state == HangarBayPrototype.State.SlotState.SlotStatus.Loading) {
                fill.fillAmount = State.LoadingProgress / State.Launchable.LoadingPreparation;
                indicator.color = orange;
                icon.sprite = State.Launchable.Icon;
                status.text = "Loading";
            } else if (state == HangarBayPrototype.State.SlotState.SlotStatus.Loaded) {
                fill.fillAmount = State.FuelingProgress / State.Launchable.FuelCapacity;
                icon.sprite = State.Launchable.Icon;
                if (State.Fueled) {
                    indicator.color = Color.green;
                    status.text = "Ready";
                } else {
                    indicator.color = Color.yellow;
                    status.text = "Refueling";
                }
            } else if (state == HangarBayPrototype.State.SlotState.SlotStatus.Launching) {
                fill.fillAmount = State.LaunchingProgress / State.Launchable.LaunchingPreparation;
                indicator.color = aqua;
                icon.sprite = State.Launchable.Icon;
                status.text = "Launching";
            } else if (state == HangarBayPrototype.State.SlotState.SlotStatus.Launched) {
                fill.fillAmount = State.FuelingProgress / State.Launchable.FuelCapacity;
                indicator.color = Color.blue;
                icon.sprite = State.Launchable.Icon;
                status.text = "In Space";
            } else if (state == HangarBayPrototype.State.SlotState.SlotStatus.Landing) {
                fill.fillAmount = State.LaunchingProgress / State.Launchable.LaunchingPreparation;
                indicator.color = aqua;
                icon.sprite = State.Launchable.Icon;
                status.text = "Landing";
            }
        }
    }
}