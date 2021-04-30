using System;

public class EntityInstance : IEntity, IInstance<IEntity>, ISavable<IEntity> {
    public StringToFloatDictionary Relations {
        get;
        private set;
    }

    public IPrototype<IEntity> Prototype {
        get;
        private set;
    }

    public void Control (IControllable controllable) {
        throw new NotImplementedException ();
    }

    public void FromPrototype (IPrototype<IEntity> prototype) {
        throw new NotImplementedException ();
    }

    public IInstance<IEntity> GetEntity () {
        throw new NotImplementedException ();
    }

    public float GetRelation (IEntity entity) {
        throw new NotImplementedException ();
    }

    public ISaveData<IEntity> GetSaveData () {
        throw new NotImplementedException ();
    }

    public void LoadSaveData (ISaveData<IEntity> data) {
        throw new NotImplementedException ();
    }
}

