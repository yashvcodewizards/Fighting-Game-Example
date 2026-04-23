using System.Collections.Generic;
using FighterBehaviour;

namespace FightTest.StateMachine
{
    public class StateMachine
    {
        private static readonly List<ITransition> _empty = new List<ITransition>();

        private Dictionary<IState, List<ITransition>> _transitions =
            new Dictionary<IState, List<ITransition>>();

        public IState CurrentState { get; private set; }

        public void Init(FighterBehaviourPackage behaviourPackage)
        {
            _transitions = behaviourPackage.Transitions;
        }

        public void Tick()
        {
            if (CurrentState == null)
            {
                return;
            }

            foreach (var transition in GetTransitions(CurrentState))
            {
                var next = transition.Evaluate();
                if (next == null)
                {
                    continue;
                }

                ChangeState(next);
                return;
            }

            CurrentState.Tick();
        }

        public void ChangeState(IState next)
        {
            ExitDeep(CurrentState);
            CurrentState = next;
            next.Enter();
        }

        private static void ExitDeep(IState state)
        {
            if (state == null)
            {
                return;
            }

            state.Exit();
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