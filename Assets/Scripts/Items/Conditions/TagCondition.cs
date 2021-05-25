using System.Collections.Generic;

public class TagCondition : ItemCondition {
    public List<ItemTag> Tags;

    public TagCondition (List<ItemTag> Tags) {
        this.Tags = Tags;
    }

    public override bool MeetsCondition (ItemSO input) {
        foreach (ItemTag tag in Tags)
            if (!input.Tags.Contains (tag))
                return false;
        return true;
    }
}
