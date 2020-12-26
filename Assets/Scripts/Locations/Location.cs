using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location {

    private readonly Transform _transform;
    private Vector3 _position;

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

}
