#nullable enable
using System;
using UnityEngine;

namespace Framework.Persistence
{
    [CreateAssetMenu(menuName = "PersistentType", fileName = "NewType")]
    public class PersistentType : ScriptableObject
    {
        public string id = Guid.NewGuid().ToString();
        public GameObject? prefab;

        public PersistentObject New(Transform? parent = null)
        {
            if (prefab == null)
            {
                throw new Exception($"No prefab associated with persistent type of id {id}");
            }
            var instantiated = Instantiate(prefab, parent).GetComponent<PersistentObject>();
            instantiated.type = this;
            return instantiated;
        }
    }
}