using System;
using UnityEngine;

[Serializable]
public class AI {

    [SerializeField] protected Structure _structure;

    public AI (Structure structure) { _structure = structure; }

    public virtual void Tick () { }

}
