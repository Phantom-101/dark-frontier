using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkFrontier.Data.Values
{
    [Serializable]
    public class MutableList<T> : MutableValue<List<T>>
    {
        public MutableList()
        {
        }

        public MutableList(List<T> baseValue) : base(baseValue)
        {
        }
    }

    [Serializable]
    public class MutableVector3List : MutableList<Vector3>
    {
        public MutableVector3List()
        {
        }

        public MutableVector3List(List<Vector3> baseValue) : base(baseValue)
        {
        }
    }
}