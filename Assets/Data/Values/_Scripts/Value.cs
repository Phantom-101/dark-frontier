using System.Collections.Generic;


namespace DarkFrontier.Data.Values
{
    public class Value<T>
    {
        private readonly List<ValueMutator<T>> _mutators = new List<ValueMutator<T>>();

        public bool AddMutator(ValueMutator<T> mutator)
        {
            int i, l;
            for (i = 0, l = _mutators.Count; i < l; i++)
            {
                if (_mutators[i].Order == mutator.Order)
                {
                    return false;
                }

                if (_mutators[i].Order > mutator.Order)
                {
                    break;
                }
            }
            _mutators.Insert(i, mutator);
            return true;
        }
    }
}
