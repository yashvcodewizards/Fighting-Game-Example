using FighterBehaviour;
using FightTest.StateMachine;
using FightTest.Systems;

namespace FightTest.States
{
    public sealed class HitStunState : IState
    {
        private readonly ColliderSet _colliders;
        private readonly HitStunTimer _timer;

        public HitStunState(ColliderSet colliders, HitStunTimer timer)
        {
            _colliders = colliders;
            _timer = timer;
        }

        public bool IsFinished => _timer.IsFinished;

        public void Enter(FighterRuntime runtime)
        {
            _colliders.EnableSet();
        }

        public void Tick(FighterRuntime runtime)
        {
            _timer.Tick();
        }

        public void Exit(FighterRuntime runtime)
        {
            _colliders.DisableSet();
        }
    }
}