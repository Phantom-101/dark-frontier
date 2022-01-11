using System.Runtime.CompilerServices;
using DarkFrontier.Foundation.Mathematics;
using UnityEngine;

public class InliningTest : MonoBehaviour {
    void Update() {
        if (Input.GetKeyUp(KeyCode.Q)) {
            var value = Math.Min(GetValueA(), GetValueB(), GetValueC());
        } else if (Input.GetKeyUp(KeyCode.W)) {
            //var value = Math.MinNonInline(GetValueA(), GetValueB(), GetValueC());
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    int GetValueA() {
        Debug.Log("A");
        return 1;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    int GetValueB() {
        Debug.Log("B");
        return 2;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    int GetValueC() {
        Debug.Log("C");
        return 3;
    }
}