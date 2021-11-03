using DarkFrontier.Foundation.Extensions;
using System;
using System.Collections.Generic;

#nullable enable
namespace DarkFrontier.Foundation.Services {
    public static class Singletons {
        private static readonly Dictionary<Type, Dictionary<string, object>> Instances = new Dictionary<Type, Dictionary<string, object>> ();

        public static T Get<T> (string aKey = "") where T : class {
            try {
                return (Instances.TryGet (typeof (T), new Dictionary<string, object> ())[aKey] as T)!;
            } catch (Exception) {
                throw new Exception ($"Service of type {typeof (T)} not found");
            }
        }

        public static bool Bind<T> (T aInstance, string aKey = "") where T : class {
            try {
                Instances.TryAdd (typeof (T), new Dictionary<string, object> ());
                Instances[typeof (T)].Add (aKey, aInstance!);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static bool Exists<T> (string key = "") where T : class => Instances.TryGet (typeof (T), new Dictionary<string, object> ()).ContainsKey (key);
    }
}
#nullable disable
