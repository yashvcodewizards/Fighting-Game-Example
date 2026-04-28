using FighterBehaviour;
using FightTest.StateMachine;
using UnityEngine;

namespace FightTest.States
{
    public sealed class DashState : IState
    {
        private readonly float _duration;
        private readonly float _speed;
        private float _lockedMoveX;
        private float _timer;

        public DashState(float speed, float duration)
        {
            _speed = speed;
            _duration = duration;
        }

        public float MoveX { get; set; }
        public bool IsFinished { get; private set; }

        public void Enter(FighterRuntime runtime)
        {
            IsFinished = false;
            _timer = 0f;
            _lockedMoveX = MoveX;
        }

        public void Tick(FighterRuntime runtime)
        {
            _timer += Time.deltaTime;
            runtime.Services.Mover.Move(_lockedMoveX, _speed);
            if (_timer >= _duration)
            {
                IsFinished = true;
            }
        }

        public void Exit(FighterRuntime runtime)
        {
            IsFinished = false;
            runtime.Services.Mover.Stop();
        }
    }
}