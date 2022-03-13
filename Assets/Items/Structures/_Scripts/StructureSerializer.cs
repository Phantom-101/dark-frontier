﻿#nullable enable
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Structures
{
    public class StructureSerializer : MonoBehaviour
    {
        [SerializeField, TextArea(1, 10)]
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
                _serialized = JsonConvert.SerializeObject(_component.Instance);
            }
        }
    }
}
