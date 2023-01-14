using System;
using Newtonsoft.Json;

namespace DarkFrontier.Data.Values
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class ConstantValue<T> : IValue<T>
    {
        public T Value { get; private init; }
        
        public ConstantValue() {}

        public ConstantValue(T value) : this() => Value = value;
        
        public static implicit operator T(ConstantValue<T> constantValue) => constantValue.Value;
        
        public static implicit operator ConstantValue<T>(T value) => new(value);
    }
    
    [Serializable]
    public class ConstantInt : ConstantValue<int>
    {
        public ConstantInt()
        {
        }

        public ConstantInt(int value) : base(value)
        {
        }
    }
    
    [Serializable]
    public class ConstantFloat : ConstantValue<float>
    {
        public ConstantFloat()
        {
        }

        public ConstantFloat(float value) : base(value)
        {
        }
    }
}