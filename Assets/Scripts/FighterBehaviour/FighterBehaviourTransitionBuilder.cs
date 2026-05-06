using System.Collections.Generic;
using FightTest.StateMachine;
using UnityEngine;

namespace FighterBehaviour
{
    public abstract class FighterBehaviourTransitionBuilder : ScriptableObject
    {
        public abstract bool CanBuildFrom(FighterBehaviourData data);
        
        public abstract Dictionary<IState, List<ITransition>> BuildTransitions(
            FighterRuntime runtime,
            FighterStateRegistry states);
    }
}