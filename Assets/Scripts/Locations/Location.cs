using System;
using UnityEngine;

[Serializable]
public class Location {
    public Transform Transform { get => transform; }
    [SerializeField] private Transform transform;
    public Vector3 Position { get => GetPosition (); }
    [SerializeField] private Vector3 position;
    public Vector3 LocalPosition { get => GetLocalPosition (); }

    public Location (Transform transform) => this.transform = transform;
    public Location (Vector3 position) => this.position = position;

    public Vector3 GetPosition () {
        if (transform != null) return transform.position;
        return position;
    }

    public Vector3 GetLocalPosition () {
        if (transform != null) return transform.localPosition;
        return position;
    }
}
