using System.Collections.Generic;

public interface IStructureSegment : IIdentifiable, IInitializable, IHitpoints {
    IStructure Structure {
        get;
    }
    List<IEquipmentData> EquipmentSlots {
        get;
    }
}
