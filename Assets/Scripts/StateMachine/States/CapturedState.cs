using Data;
using FighterBehaviour;
using FightTest.StateMachine;

namespace FightTest.States
{
    public sealed class CapturedState : IState
    {
        private readonly ColliderProfile _colliderProfile;
        private readonly string _animationLabel;

        public CapturedState(ColliderProfile colliderProfile, string animationLabel = "Captured")
        {
            _colliderProfile = colliderProfile;
            _animationLabel = animationLabel;
        }

        public void Enter(FighterRuntime runtime)
        {
            runtime.Services.ColliderManager.ApplyProfile(_colliderProfile);
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