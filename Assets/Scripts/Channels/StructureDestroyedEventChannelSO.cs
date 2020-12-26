﻿using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Events/Structure Destroyed Event Channel")]
public class StructureDestroyedEventChannelSO : ScriptableObject {

    public UnityAction<Structure> OnStructureDestroyed;

    public void RaiseEvent (Structure destroyedStructure) {

        if (OnStructureDestroyed != null) OnStructureDestroyed.Invoke (destroyedStructure);

    }

}
