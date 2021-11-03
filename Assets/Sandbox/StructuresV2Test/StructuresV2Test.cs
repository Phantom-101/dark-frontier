using DarkFrontier.Items.Prototypes;
using DarkFrontier.Structures;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Sandbox.StructuresV2Test {
    public class StructuresV2Test : MonoBehaviour {
        public StructurePrototype uPrototype;
        public int uSerializationCount;

        private void Update () {
            for (int lCounter = 0; lCounter < uSerializationCount; lCounter++) {
                Structure.State lState = new Structure.State (uPrototype);
                lState.uStats.Add (new Structure.Stats.Modifier.Derived (0, float.PositiveInfinity, 1));

                string lSerialized = JsonConvert.SerializeObject (lState, new Structure.State.Converter ());

                Structure.State lDeserialized = JsonConvert.DeserializeObject<Structure.State> (lSerialized, new Structure.State.Converter ());
            }
        }
    }
}
