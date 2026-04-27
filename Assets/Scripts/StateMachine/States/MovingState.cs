using System;
using FighterBehaviour;
using FightTest.StateMachine;
using FightTest.Systems;

namespace FightTest.States
{
    public sealed class MovingState : IState
    {
        private readonly Func<float> _getMoveX;
        private readonly Func<float> _getSpeed;

        public float MoveX { get; private set; }
        public float Speed { get; private set;}

        public MovingState(Func<float> getMoveX,
            Func<float> getSpeed)
        {
            _getMoveX = getMoveX;
            _getSpeed = getSpeed;
        }

        public void Enter(FighterRuntime runtime)
        {
        }

        public void Tick(FighterRuntime runtime)
        {
            MoveX = _getMoveX();
            Speed = _getSpeed();
            
            runtime.Services.Mover.Move(MoveX, Speed);
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}