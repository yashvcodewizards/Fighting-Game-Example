using FightTest.StateMachine;
using FightTest.Systems;

namespace FightTest.States
{
    public sealed class KnockedDownState : IState
    {
        private readonly ColliderSet _colliders;
        private readonly int _durationTicks;
        private int _remainingTicks;

        public KnockedDownState(ColliderSet colliders, int durationTicks = 30)
        {
            _colliders = colliders;
            _durationTicks = durationTicks;
        }

        public bool IsFinished { get; private set; }
        public bool IsInvulnerable => true;

        public void Enter()
        {
            IsFinished = false;
            _remainingTicks = _durationTicks;
            _colliders.EnableSet();
        }

        public void Tick()
        {
            if (_durationTicks <= 0)
            {
                return;
            }

            _remainingTicks--;
            if (_remainingTicks <= 0)
            {
                IsFinished = true;
            }
        }

        public void Exit()
        {
            _colliders.DisableSet();
            IsFinished = false;
        }
    }
}