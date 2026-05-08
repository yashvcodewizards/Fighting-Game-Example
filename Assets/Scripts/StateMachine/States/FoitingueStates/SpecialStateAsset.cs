using FighterBehaviour;
using FightTest.StateMachine;
using UnityEngine;

namespace FightTest.States.FoitingueStates
{
    public abstract class SpecialStateAsset : ScriptableObject, IState
    {
        public virtual IState CreateRuntimeState()
        {
            return Instantiate(this);
        }

        public abstract void Enter(FighterRuntime runtime);
        public abstract void Tick(FighterRuntime runtime);
        public abstract void Exit(FighterRuntime runtime);
    }
}