public interface IEntity : IIdentifiable, IInfo {
    StringToFloatDictionary Relations {
        get;
    }

    IInstance<IEntity> GetEntity ();

    float GetRelation (IEntity entity);

    void Control (IControllable controllable);
}
