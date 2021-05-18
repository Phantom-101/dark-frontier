using System.Collections.Generic;

public interface IStructure : IIdentifiable, IInitializable, IInfo, IControllable, IHitpoints, IScanner, ITargetable {
    List<IStructureSegment> Segments {
        get;
    }
}
