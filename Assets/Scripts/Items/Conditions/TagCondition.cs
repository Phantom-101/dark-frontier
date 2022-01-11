using System.Collections.Generic;
using DarkFrontier.Structures;

namespace DarkFrontier.Items.Conditions {
    public class TagCondition : ItemCondition {
        public List<ItemTag> Tags;

        public TagCondition (List<ItemTag> Tags) {
            this.Tags = Tags;
        }

        public override bool MeetsCondition (ItemPrototype input) {
            foreach (ItemTag tag in Tags)
                if (!input.Tags.Contains (tag))
                    return false;
            return true;
        }
    }
}
