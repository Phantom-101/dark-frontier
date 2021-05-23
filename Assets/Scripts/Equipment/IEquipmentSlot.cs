public interface IEquipmentSlot : IIdentifiable, IInitializable, IUpdatable {
    IEquipmentData Data {
        get;
    }
}
