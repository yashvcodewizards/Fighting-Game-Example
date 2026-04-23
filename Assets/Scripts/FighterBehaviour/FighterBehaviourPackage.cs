using System.Collections.Generic;
using FightTest.StateMachine;

namespace FighterBehaviour
{
    public class FighterBehaviourPackage
    {
        public IState InitialState { get; }
        public Dictionary<IState, List<ITransition>> Transitions;

        public FighterBehaviourPackage(
            IState initialState,
            Dictionary<IState, List<ITransition>> transitions)
        {
            InitialState = initialState;
            Transitions = transitions;
        }
    }
}