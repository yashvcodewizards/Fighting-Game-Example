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
        private readonly ColliderProfile _colliderProfile;
        private readonly string _animationLabel;
        
        public SimpleState(ColliderProfile colliderProfile, string animationLabel = null)
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
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}