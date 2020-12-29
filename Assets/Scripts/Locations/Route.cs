using System.Collections.Generic;
using UnityEngine;

public class Route {

    private readonly Queue<Location> _waypoints = new Queue<Location> ();

    public Route (List<Location> locations) {

        foreach (Location location in locations) _waypoints.Enqueue (location);

    }

    public Route (List<Vector3> positions) {

        List<Location> locations = new List<Location> ();
        positions.ForEach (position => {
            locations.Add (new Location (position));
        });

    }

    public Route (List<Transform> transforms) {

        List<Location> locations = new List<Location> ();
        transforms.ForEach (transform => {
            locations.Add (new Location (transform));
        });

    }

    public void AddWaypoint (Location location) { _waypoints.Enqueue (location); }

    public Location GetNextWaypoint () { return _waypoints.Peek (); }

    public void ReachedWaypoint () { _waypoints.Dequeue (); }

    public int GetWaypointsCount () { return _waypoints.Count; }

    public Queue<Location> GetWaypoints () { return _waypoints; }

}
