using System.Collections;
using UnityEngine;

public class ColliderRemover : MonoBehaviour
{
    public void Execute()
    {
        StartCoroutine(Remove());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    private static IEnumerator Remove()
    {
        var colliders = FindObjectsOfType<Collider>();
        var work = 1000;
        for (int i = 0, l = colliders.Length; i < l; i++)
        {
            DestroyImmediate(colliders[i]);
            Debug.Log($"{l - i - 1} left");
            work--;
            if (work == 0)
            {
                work = 1000;
                yield return null;
            }
        }
        Debug.Log("Done!");
    }
}
