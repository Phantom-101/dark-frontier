#nullable enable
using DarkFrontier.Foundation.Extensions;
using System;
using System.Collections.Generic;

namespace DarkFrontier.Foundation.Services {
    public static class Singletons {
        private static readonly Dictionary<Type, Dictionary<string, object>> _Instances = new();

        public static T Get<T> (string key = "") where T : class {
            try {
                return (_Instances.TryGet (typeof (T), new Dictionary<string, object> ())![key] as T)!;
            } catch (Exception) {
                throw new Exception ($"Service of type {typeof (T)} not found");
            }
        }

        public static void Bind<T>(T instance, string key = "") where T : class {
            _Instances.TryAdd (typeof (T), new Dictionary<string, object> ());
            _Instances[typeof (T)][key] = instance;
        }

        public static bool Exists<T> (string key = "") where T : class => _Instances.ContainsKey(typeof(T)) && _Instances[typeof(T)].ContainsKey(key);
    }
}

