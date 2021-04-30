public interface IEntity {
    StringToFloatDictionary Relations {
        get;
    }

    IInstance<IEntity> GetEntity ();

    float GetRelation (IEntity entity);

    void Control (IControllable controllable);
}
