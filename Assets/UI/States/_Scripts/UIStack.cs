#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace DarkFrontier.UI.States
{
    public class UIStack : MonoBehaviour
    {
        private readonly List<New.UIState> _states = new();

        public New.UIState? Peek() => _states.Count == 0 ? null : _states[^1];

        public void Pop()
        {
            if(_states.Count != 0)
            {
                _states[^1].OnStateExit();
                _states.RemoveAt(_states.Count - 1);
                if(_states.Count != 0)
                {
                    _states[^1].OnStateEnter();
                }
            }
        }

        public void Push(New.UIState state)
        {
            if(_states.Count != 0) _states[^1].OnStateExit();
            _states.Add(state);
            state.OnStateEnter();
        }

        public void Replace(New.UIState state)
        {
            if(_states.Count != 0)
            {
                _states[^1].OnStateExit();
                _states.RemoveAt(_states.Count - 1);
            }
            _states.Add(state);
            state.OnStateEnter();
        }

        public void Tick()
        {
            if(_states.Count != 0) _states[^1].OnStateRemain();
        }
    }
}