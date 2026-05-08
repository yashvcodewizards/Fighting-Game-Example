using Data;
using FighterBehaviour;
using FightTest.StateMachine;

namespace FightTest.States
{
    public sealed class CapturedState : IState
    {
        private readonly BoxProfile _boxProfile;
        private readonly string _animationLabel;

        public CapturedState(BoxProfile boxProfile, string animationLabel = "Captured")
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
            // intentionally empty
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}