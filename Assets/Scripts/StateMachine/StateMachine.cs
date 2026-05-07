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
            
            ExitCurrentState();
            
            CurrentState = next;
            next.Enter(_runtime);
        }

        private void ExitCurrentState()
        {
            if (CurrentState == null)
            {
                return;
            }

            CurrentState.Exit(_runtime);
        }
        
        public void StopCurrentState()
        {
            ExitCurrentState();
            CurrentState = null;
        }

        private List<ITransition> GetTransitions(IState state)
        {
            return _transitions.TryGetValue(state, out var list) ? list : _empty;
        }
    }
}