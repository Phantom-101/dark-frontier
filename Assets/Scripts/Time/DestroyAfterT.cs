﻿using UnityEngine;

public class DestroyAfterT : MonoBehaviour {
    public float Time;

    private void Awake () {
        Destroy (gameObject, Time);
    }
}