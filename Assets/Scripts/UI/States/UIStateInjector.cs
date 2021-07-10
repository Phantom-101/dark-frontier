using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIStateInjector : MonoBehaviour {
    public string Name;
    public CanvasGroup Group;
    public bool ShowBelow;
    public bool AlwaysShown;
    [Tooltip ("Lower order injectors get injected before higher order injectors")]
    public int Order;

    private void Start () {
        // Get all injectors
        List<UIStateInjector> injectors = FindObjectsOfType<UIStateInjector> ().ToList ();
        // Sort based on priority
        // Lower order injectors get injected before higher order injectors
        // In other words, lower order states are below higher order injectors
        injectors.Sort ((i1, i2) => i1.Order.CompareTo (i2.Order));
        // Cache reference to UI state manager
        UIStateManager uiStateManager = UIStateManager.Instance;
        // Iterate through injectors in order and inject state
        foreach (UIStateInjector injector in injectors) {
            UIStateManager.Instance.AddState (new UIState {
                Name = injector.Name,
                Group = injector.Group,
                ShowBelow = injector.ShowBelow,
                AlwaysShow = injector.AlwaysShown,
            });
            // Destroy injector
            Destroy (injector);
        }
    }
}
