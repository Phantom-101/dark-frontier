using Newtonsoft.Json;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Items.Structures
{
    class StructureSerializer : MonoBehaviour
    {
        [SerializeField]
        [TextArea(1, 10)]
        private string _serialized = "";

        private StructureComponent? _component;

        private void Start()
        {
            _component = GetComponent<StructureComponent>();
        }

        private void Update()
        {
            if (_component != null)
            {
                _serialized = JsonConvert.SerializeObject(_component.instance);
            }
        }
    }
}
#nullable restore