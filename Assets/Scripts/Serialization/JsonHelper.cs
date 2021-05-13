using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper {
    public static T[] ArrayFromJson<T> (string json) {
        ArrayWrapper<T> wrapper = JsonUtility.FromJson<ArrayWrapper<T>> (json);
        return wrapper.Items;
    }

    public static string ToJson<T> (T[] array, bool prettyPrint = false) {
        ArrayWrapper<T> wrapper = new ArrayWrapper<T> {
            Items = array
        };
        return JsonUtility.ToJson (wrapper, prettyPrint);
    }

    [Serializable]
    private class ArrayWrapper<T> {
        public T[] Items;
    }

    public static List<T> ListFromJson<T> (string json) {
        ListWrapper<T> wrapper = JsonUtility.FromJson<ListWrapper<T>> (json);
        return wrapper.Items;
    }

    public static string ToJson<T> (List<T> list, bool prettyPrint = false) {
        ListWrapper<T> wrapper = new ListWrapper<T> {
            Items = list
        };
        return JsonUtility.ToJson (wrapper, prettyPrint);
    }

    [Serializable]
    private class ListWrapper<T> {
        public List<T> Items;
    }

}