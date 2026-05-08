using System.Collections.Generic;
using FightTest.StateMachine;

namespace FighterBehaviour
{
    /// <summary>
    /// Built result of a fighter behaviour definition.
    /// Contains the initial state and transition graph needed by the state machine.
    /// </summary>
    public class FighterBehaviourPackage
    {
        public IState InitialState { get; }
        public Dictionary<IState, List<ITransition>> Transitions { get; }
        public FighterStateRegistry StateRegistry { get; set; }

        public FighterBehaviourPackage(
            IState initialState,
            Dictionary<IState, List<ITransition>> transitions,
            FighterStateRegistry stateRegistry)
        {
            InitialState = initialState;
            Transitions = transitions;
            StateRegistry = stateRegistry;
        }
    }
}