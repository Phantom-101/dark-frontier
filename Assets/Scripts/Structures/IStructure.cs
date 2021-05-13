using System.Collections.Generic;

public interface IStructure : IIdentifiable, IInitializable, IInfo, IControllable, IHitpoints, ITargetable {
    List<IStructureSegment> Segments {
        get;
    }
}
