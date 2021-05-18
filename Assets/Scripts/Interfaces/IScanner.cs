using System.Collections.Generic;

public interface IScanner {
    IStat ScannerStrength {
        get;
    }
    Dictionary<ITargetable, float> ActiveLocks {
        get;
    }
}
