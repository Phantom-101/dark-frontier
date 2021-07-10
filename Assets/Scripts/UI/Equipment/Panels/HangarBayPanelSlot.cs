using UnityEngine;
using UnityEngine.UI;

public class HangarBayPanelSlot : MonoBehaviour {
    public HangarBayLaunchSlot LaunchSlot;
    [SerializeField] private Button button;
    [SerializeField] private Image fill;
    [SerializeField] private Image indicator;
    [SerializeField] private Image icon;
    [SerializeField] private Text status;

    private Color orange = new Color (1, 0.5f, 0);
    private Color aqua = new Color (0, 1, 1);

    private void Start () {
        button.onClick.AddListener (() => {
            HangarBayLaunchSlotState state = LaunchSlot.State;
            if (state == HangarBayLaunchSlotState.Loading || state == HangarBayLaunchSlotState.Loaded) LaunchSlot.State = HangarBayLaunchSlotState.Unloading;
        });
    }

    private void Update () {
        HangarBayLaunchSlotState state = LaunchSlot.State;
        if (state == HangarBayLaunchSlotState.Unloaded) {
            fill.fillAmount = 0;
            indicator.color = Color.red;
            icon.sprite = null;
            status.text = "Empty";
        } else if (state == HangarBayLaunchSlotState.Unloading) {
            fill.fillAmount = LaunchSlot.LoadingProgress / LaunchSlot.Launchable.LoadingPreparation;
            indicator.color = orange;
            icon.sprite = LaunchSlot.Launchable.Icon;
            status.text = "Unloading";
        } else if (state == HangarBayLaunchSlotState.Loading) {
            fill.fillAmount = LaunchSlot.LoadingProgress / LaunchSlot.Launchable.LoadingPreparation;
            indicator.color = orange;
            icon.sprite = LaunchSlot.Launchable.Icon;
            status.text = "Loading";
        } else if (state == HangarBayLaunchSlotState.Loaded) {
            fill.fillAmount = LaunchSlot.FuelingProgress / LaunchSlot.Launchable.FuelCapacity;
            icon.sprite = LaunchSlot.Launchable.Icon;
            if (LaunchSlot.Fueled) {
                indicator.color = Color.green;
                status.text = "Ready";
            } else {
                indicator.color = Color.yellow;
                status.text = "Refueling";
            }
        } else if (state == HangarBayLaunchSlotState.Launching) {
            fill.fillAmount = LaunchSlot.LaunchingProgress / LaunchSlot.Launchable.LaunchingPreparation;
            indicator.color = aqua;
            icon.sprite = LaunchSlot.Launchable.Icon;
            status.text = "Launching";
        } else if (state == HangarBayLaunchSlotState.Launched) {
            fill.fillAmount = LaunchSlot.Structure.Hull / LaunchSlot.Launchable.Stats.GetBaseValue (StatNames.MaxHull, 1);
            indicator.color = Color.blue;
            icon.sprite = LaunchSlot.Launchable.Icon;
            status.text = "In Space";
        } else if (state == HangarBayLaunchSlotState.Landing) {
            fill.fillAmount = LaunchSlot.LaunchingProgress / LaunchSlot.Launchable.LaunchingPreparation;
            indicator.color = aqua;
            icon.sprite = LaunchSlot.Launchable.Icon;
            status.text = "Landing";
        }
    }
}
