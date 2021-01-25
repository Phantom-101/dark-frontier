using System;
using UnityEngine;

[Serializable]
public class Location {

    [SerializeField] private Transform _transform;
    [SerializeField] private Vector3 _position;

    public Location (Transform transform) {

        _transform = transform;

    }

    public Location (Vector3 position) {

        _position = position;

    }

    public Transform GetTransform () {

        return _transform;

    }

    public Vector3 GetPosition () {

        if (_transform != null) return _transform.position;

        return _position;

    }

    public Vector3 GetLocalPosition () {

        if (_transform != null) return _transform.localPosition;

        return _position;

    }

}
