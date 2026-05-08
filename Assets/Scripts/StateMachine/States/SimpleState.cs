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
        private readonly BoxProfile _boxProfile;
        private readonly string _animationLabel;
        
        public SimpleState(BoxProfile boxProfile, string animationLabel = null)
        {
            _boxProfile = boxProfile;
            _animationLabel = animationLabel;
        }
        
        public void Enter(FighterRuntime runtime)
        {
            runtime.Services.HitBoxManager.ApplyProfile(_boxProfile);
            runtime.Services.Presentation.Play(_animationLabel);
        }

        public void Tick(FighterRuntime runtime)
        {
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}