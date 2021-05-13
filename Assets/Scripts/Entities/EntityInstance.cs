using System;

public class EntityInstance : IEntity, IInstance<IEntity>, ISerializable<IEntity> {
    public StringToFloatDictionary Relations {
        get;
        private set;
    }
    public IPrototype<IEntity> Prototype {
        get;
        private set;
    }
    public string Id {
        get;
        private set;
    }
    public string Name {
        get;
        private set;
    }
    public string Description {
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

    public ISerialized<IEntity> GetSerialized () {
        throw new NotImplementedException ();
    }

    public void LoadSaveData (string json) {
        throw new NotImplementedException ();
    }
}

