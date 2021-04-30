using System;
using UnityEngine;

public class EntityPrototype : ScriptableObject, IEntity, IPrototype<IEntity> {
    public StringToFloatDictionary Relations {
        get;
        private set;
    }

    public IInstance<IEntity> Instance {
        get;
        private set;
    }

    public void Control (IControllable controllable) {
        throw new NotImplementedException ();
    }

    public IInstance<IEntity> GetEntity () {
        throw new NotImplementedException ();
    }

    public float GetRelation (IEntity entity) {
        throw new NotImplementedException ();
    }
}

