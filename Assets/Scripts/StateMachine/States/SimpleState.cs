using Data;
using FighterBehaviour;
using FightTest.StateMachine;

namespace FightTest.States
{
    /// <summary>
    /// Basic state with no special behaviour.
    /// Used for states that only need to exist as part of the state graph.
    /// </summary>
    public sealed class SimpleState : IState
    {
        private BoxProfile _boxProfile;
        
        public SimpleState(BoxProfile boxProfile)
        {
            _boxProfile = boxProfile;
        }
        
        public void Enter(FighterRuntime runtime)
        {
            runtime.Services.HitBoxManager.ApplyProfile(_boxProfile);
        }

        public void Tick(FighterRuntime runtime)
        {
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}