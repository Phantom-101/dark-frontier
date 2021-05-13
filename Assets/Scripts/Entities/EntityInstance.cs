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

    public IInstance<IEntity> GetEntity () { return this; }

    public float GetRelation (IEntity entity) { return Relations.ContainsKey (entity.Id) ? Relations[entity.Id] : 0; }

    public ISerialized<IEntity> GetSerialized () {
        throw new NotImplementedException ();
    }
}

