using System;
using System.Collections.Generic;
using FightTest.StateMachine;

namespace FighterBehaviour
{
    public sealed class StateRegistry
    {
        private readonly Dictionary<string, IState> _states = new Dictionary<string, IState>();

        public IState InitialState { get; private set; }

        public void Add(string key, IState state)
        {
            _states[key] = state;
        }

        public void SetInitial(string key)
        {
            InitialState = Get(key);
        }
        
        public bool Has(string key)
        {
            return _states.ContainsKey(key);
        }

        public IState Get(string key)
        {
            if (!_states.TryGetValue(key, out var state))
            {
                throw new Exception($"State '{key}' was not found.");
            }

            return state;
        }

        public T Get<T>(string key) where T : class, IState
        {
            return Get(key) as T
                   ?? throw new Exception($"State '{key}' is not a {typeof(T).Name}.");
        }
    }
}