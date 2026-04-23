using System.Collections.Generic;
using FightTest.StateMachine;

namespace FighterBehaviour
{
    public class FighterBehaviourPackage
    {
        public Dictionary<IState, List<ITransition>> Transitions;

        public FighterBehaviourPackage(Dictionary<IState, List<ITransition>> transitions)
        {
            Transitions = transitions;
        }
    }
}