using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DockingBay {
    public int MaxCount { get => maxCount; }
    [SerializeField] private int maxCount;

    public float MaxVolume { get => maxVolume; }
    [SerializeField] private float maxVolume;

    public int DockedCount { get => dockers.Count; }
    private readonly HashSet<Structure> dockers = new HashSet<Structure> ();
    public List<Structure> Dockers { get => dockersList ?? (dockersList = dockers.ToList ()); }
    [SerializeField] private List<Structure> dockersList;

    public float DockedVolume { get => dockedVolume ?? (dockedVolume = RecalculateDockedVolume ()).Value; }
    [SerializeField] private float? dockedVolume;

    public int Precision { get => precision; set { precision = value; RecalculateDockedVolume (); } }
    [SerializeField] private int precision;

    public bool HasDocker (Structure docker) => dockers.Contains (docker);

    public bool CanAccept (Structure docker) {
        if (DockedCount == maxCount) return false;
        if (maxVolume >= 0 && maxVolume - DockedVolume < docker.Profile.Volume) return false;
        if (dockers.Contains (docker)) return false;
        return true;
    }

    public bool TryAccept (Structure docker) {
        if (!CanAccept (docker)) return false;
        dockers.Add (docker);
        dockedVolume = null;
        dockersList = null;
        return true;
    }

    public bool CanRelease (Structure docker) => dockers.Contains (docker);

    public bool TryRelease (Structure docker) {
        if (!CanRelease (docker)) return false;
        dockers.Remove (docker);
        dockedVolume = null;
        dockersList = null;
        return true;
    }

    private float RoundToPrecision (float value) => (float) Math.Round (value, precision);
    private float GetVolume (Structure structure) => RoundToPrecision (structure.Profile.Volume);

    private float RecalculateDockedVolume () {
        dockedVolume = 0;
        foreach (Structure docked in dockers) dockedVolume = RoundToPrecision (dockedVolume.Value + GetVolume (docked));
        return dockedVolume.Value;
    }

    public DockingBay Copy () {
        DockingBay ret = new DockingBay {
            maxCount = maxCount,
            maxVolume = maxVolume,
            precision = precision
        };
        return ret;
    }
}
