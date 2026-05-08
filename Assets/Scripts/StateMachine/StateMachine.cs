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

        private StateRegistry _stateRegistry;
        public StateRegistry StateRegistry => _stateRegistry;

        public IState CurrentState { get; private set; }

        public void Init(FighterBehaviourPackage behaviourPackage, FighterRuntime runtime)
        {
            _transitions = behaviourPackage.Transitions;
            _runtime = runtime;
            _stateRegistry = behaviourPackage.StateRegistry;
            
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
            _runtime.Services.StateTimer.Tick();
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
            
            _runtime.Services.StateTimer.Reset();
            
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

        public List<ITransition> GetTransitions(IState state)
        {
            return _transitions.TryGetValue(state, out var list) ? list : _empty;
        }
    }
}