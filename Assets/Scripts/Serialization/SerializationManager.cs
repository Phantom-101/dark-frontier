using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager {

    public static bool Save (string directoryPath, string saveName, string saveData) {

        if (!Directory.Exists (directoryPath)) Directory.CreateDirectory (directoryPath);

        string path = directoryPath + "/" + saveName;
        FileStream file = File.Create (path);
        file.Close ();
        File.WriteAllText (path, saveData);

        return true;

    }

    public static string Load (string path) {

        if (!File.Exists (path)) return "";

        return File.ReadAllText (path);

    }

    [Obsolete ("Do not use BinaryFormatter! It is prone to security vulnerabilities. Use JsonUtility instead.", true)]
    public static BinaryFormatter GetBinaryFormatter () {

        BinaryFormatter formatter = new BinaryFormatter ();

        SurrogateSelector selector = new SurrogateSelector ();

        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate ();
        QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate ();

        selector.AddSurrogate (typeof (Vector3), new StreamingContext (StreamingContextStates.All), vector3Surrogate);
        selector.AddSurrogate (typeof (Quaternion), new StreamingContext (StreamingContextStates.All), quaternionSurrogate);

        formatter.SurrogateSelector = selector;

        return formatter;

    }

}
