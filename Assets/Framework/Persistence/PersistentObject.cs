#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Persistence {
    public class PersistentObject : MonoBehaviour {
        public PersistentType type = null!;
        public string id = string.Empty;

        protected virtual void Awake() {
            if (string.IsNullOrEmpty(id)) {
                id = Guid.NewGuid().ToString();
            }
        }

        public virtual PersistentData Save(PersistentData? data = null) {
            data ??= new PersistentData();
            data.typeId = type.id;
            data.id = id;
            var t = transform;
            data.Add("position", Vector3ToFloatArray(t.localPosition));
            data.Add("rotation", Vector3ToFloatArray(t.localEulerAngles));
            return data;
        }

        public virtual void Load(PersistentData data) {
            var t = transform;
            t.localPosition = FloatArrayToVector3(data.GetArray<float>("position"));
            t.localEulerAngles = FloatArrayToVector3(data.GetArray<float>("rotation"));
        }

        private static float[] Vector3ToFloatArray(Vector3 v) {
            return new[] { v.x, v.y, v.z };
        }

        private static Vector3 FloatArrayToVector3(IReadOnlyList<float> a) {
            return new Vector3(a[0], a[1], a[2]);
        }
    }
}