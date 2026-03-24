using FightTest.StateMachine;
using FightTest.Systems;

namespace FightTest.States
{
    public sealed class BlockStunState : IState
    {
        private readonly ColliderSet _colliders;
        private readonly CharacterMover _mover;
        private int _remainingTicks;

        public BlockStunState(CharacterMover mover, ColliderSet colliders)
        {
            _mover = mover;
            _colliders = colliders;
        }

        public bool IsFinished { get; private set; }

        public void Enter()
        {
            _mover.Stop();
            _colliders.EnableSet();
            IsFinished = false;
        }

        public void Tick()
        {
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

        public void Configure(int frames)
        {
            _remainingTicks = frames;
        }
    }
}