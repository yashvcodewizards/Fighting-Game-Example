using System.Collections.Generic;
using FighterBehaviour;
using UnityEngine;

namespace FightTest.StateMachine
{
    public class StateMachine
    {
        private static readonly List<ITransition> _empty = new List<ITransition>();

        private Dictionary<IState, List<ITransition>> _transitions =
            new Dictionary<IState, List<ITransition>>();

        private FighterRuntime _runtime;

        public IState CurrentState { get; private set; }

        public void Init(FighterBehaviourPackage behaviourPackage, FighterRuntime runtime)
        {
            _transitions = behaviourPackage.Transitions;
            _runtime = runtime;
            
            ChangeState(behaviourPackage.InitialState);
        }

        public void Tick()
        {
            if (CurrentState == null)
            {
                return;
            }

            var currentTransitions = GetTransitions(CurrentState);
            foreach (var transition in currentTransitions)
            {
                var next = transition.Evaluate();
                if (next == null)
                {
                    continue;
                }

                ChangeState(next);
                return;
            }

            CurrentState.Tick(_runtime);
        }

        public void ChangeState(IState next)
        {
            if (next == null)
            {
                Debug.LogWarning("Tried to change to null state.");
                return;
            }

            Debug.Log($"State change: {CurrentState?.GetType().Name ?? "NULL"} -> {next.GetType().Name}");

            
            Exit(CurrentState);
            CurrentState = next;
            next.Enter(_runtime);
        }

        private void Exit(IState state)
        {
            if (state == null)
            {
                return;
            }

            state.Exit(_runtime);
        }

        public void RegisterTransitions(IState state, params ITransition[] transitions)
        {
            if (!_transitions.ContainsKey(state))
            {
                _transitions[state] = new List<ITransition>();
            }

            _transitions[state].AddRange(transitions);
        }

        private List<ITransition> GetTransitions(IState state)
        {
            return _transitions.TryGetValue(state, out var list) ? list : _empty;
        }
    }
}