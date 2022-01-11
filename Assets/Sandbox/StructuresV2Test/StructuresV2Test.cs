using DarkFrontier.Equipment;
using DarkFrontier.Structures;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Sandbox.StructuresV2Test {
    public class StructuresV2Test : MonoBehaviour {
        public StructurePrototype uPrototype;

        private void Start() {
            var lState = new Structure.State(uPrototype);
            lState.uStats.Add(new HangarBayPrototype.Penalty(1, 1));

            var lSerialized = JsonConvert.SerializeObject(lState, new Structure.State.Converter());

            var lDeserialized = JsonConvert.DeserializeObject<Structure.State>(lSerialized, new Structure.State.Converter());
        }
    }
}