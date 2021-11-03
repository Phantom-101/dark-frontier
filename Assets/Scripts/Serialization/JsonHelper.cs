using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Serialization {
    public static class JsonHelper {
        public static T[] ArrayFromJson<T> (string json) {
            ArrayWrapper<T> wrapper = JsonUtility.FromJson<ArrayWrapper<T>> (json);
            return wrapper.Items;
        }

        public static string ToJson<T> (T[] array, Formatting formatting = Formatting.None) {
            ArrayWrapper<T> wrapper = new ArrayWrapper<T> {
                Items = array
            };
            return JsonConvert.SerializeObject (wrapper, formatting, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
            });
        }

        [Serializable]
        private class ArrayWrapper<T> {
            public T[] Items;
        }

        public static List<T> ListFromJson<T> (string json) {
            ListWrapper<T> wrapper = JsonUtility.FromJson<ListWrapper<T>> (json);
            return wrapper.Items;
        }

        public static string ToJson<T> (List<T> list, Formatting formatting = Formatting.None) {
            ListWrapper<T> wrapper = new ListWrapper<T> {
                Items = list
            };
            return JsonConvert.SerializeObject (wrapper, formatting, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
            });
        }

        [Serializable]
        private class ListWrapper<T> {
            public List<T> Items;
        }

    }
}