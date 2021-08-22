using DarkFrontier.Factions;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[Serializable]
public class DockingBayList {
    public List<DockingBay> DockingBays { get => dockingBays; }
    [SerializeField] private List<DockingBay> dockingBays = new List<DockingBay> ();

    public int DockedCount { get => dockers.Count; }
    private readonly HashSet<Structure> dockers = new HashSet<Structure> ();
    public List<Structure> Dockers { get => dockersList ?? (dockersList = dockers.ToList ()); }
    [SerializeField] private List<Structure> dockersList;

    public Structure Structure { get => structure; }
    [SerializeField] private Structure structure;

    private FactionManager factionManager;

    public DockingBayList (Structure structure) {
        this.structure = structure;
    }

    [Inject]
    public void Construct (FactionManager factionManager) {
        this.factionManager = factionManager;
    }

    public bool CanAccept (Structure docker) {
        // In the same sector or already docked?
        if (docker.transform.parent != structure.transform.parent) return false;
        // Good relations?
        if (structure.Faction == null || structure.Faction.Value (factionManager.Registry.Find).IsEnemy (docker.Faction.Id.Value)) return false;
        // Within range?
        if (NavigationManager.Instance.GetLocalDistance (structure, docker) > 50) return false;
        // Already children?
        if (dockers.Contains (docker)) return false;
        // Enough space?
        foreach (DockingBay bay in dockingBays) {
            if (bay.CanAccept (docker)) return true;
        }
        return false;
    }

    public bool TryAccept (Structure docker) {
        if (!CanAccept (docker)) return false;
        foreach (DockingBay bay in dockingBays) {
            if (bay.TryAccept (docker)) {
                dockers.Add (docker);
                dockersList = null;
                docker.OnDocked (structure);
                return true;
            }
        }
        return false;
    }

    public bool CanRelease (Structure docker) => dockers.Contains (docker);

    public bool TryRelease (Structure docker) {
        if (!CanRelease (docker)) return false;
        foreach (DockingBay bay in dockingBays) {
            if (bay.TryRelease (docker)) {
                dockers.Remove (docker);
                dockersList = null;
                docker.OnUndocked (structure);
                return true;
            }
        }
        return false;
    }

    public DockingBayList Copy (Structure target) {
        DockingBayList ret = new DockingBayList (target);
        dockingBays.ForEach ((bay) => ret.DockingBays.Add (bay.Copy ()));
        return ret;
    }
}
