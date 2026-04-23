using System;
using FightTest.StateMachine;
using FightTest.Systems;

namespace FightTest.States
{
    public sealed class MovingState : IState
    {
        private readonly ColliderSet _colliders;
        private readonly CharacterMover _mover;
        private readonly Func<float> _getMoveX;
        private readonly Func<float> _getSpeed;

        public float MoveX { get; private set; }
        public float Speed { get; private set;}

        public MovingState(CharacterMover mover, ColliderSet colliders, Func<float> getMoveX,
            Func<float> getSpeed)
        {
            _mover = mover;
            _colliders = colliders;
            _getMoveX = getMoveX;
            _getSpeed = getSpeed;
        }

        public void Enter()
        {
            _colliders.EnableSet();
        }

        public void Tick()
        {
            MoveX = _getMoveX();
            Speed = _getSpeed();
            
            _mover.Move(MoveX, Speed);
        }

        public void Exit()
        {
            _mover.Stop();
            _colliders.DisableSet();
        }
    }
}