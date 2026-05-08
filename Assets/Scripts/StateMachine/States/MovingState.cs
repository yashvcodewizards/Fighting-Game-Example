using System;
using FighterBehaviour;
using FightTest.StateMachine;

namespace FightTest.States
{
    public sealed class MovingState : IState
    {
        private readonly string _animationLabel;

        private readonly Func<FighterRuntime, float> _getMoveX;
        private readonly Func<FighterRuntime, float> _getSpeed;

        public float MoveX { get; private set; }
        public float Speed { get; private set; }

        public MovingState(
            Func<FighterRuntime, float> getMoveX,
            Func<FighterRuntime, float> getSpeed,
            string animationLabel = null)
        {
            _getMoveX = getMoveX;
            _getSpeed = getSpeed;
            _animationLabel = animationLabel;
        }

        public void Enter(FighterRuntime runtime)
        {
            runtime.Services.Presentation.Play(_animationLabel);
        }

        public void Tick(FighterRuntime runtime)
        {
            MoveX = _getMoveX(runtime);
            Speed = _getSpeed(runtime);

            runtime.Services.Mover.Move(MoveX, Speed);
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}