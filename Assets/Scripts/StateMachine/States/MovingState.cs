using System;
using FighterBehaviour;
using FightTest.StateMachine;
using FightTest.Systems;

namespace FightTest.States
{
    public sealed class MovingState : IState
    {
        private readonly Func<FighterRuntime,float> _getMoveX;
        private readonly Func<FighterRuntime,float> _getSpeed;

        public float MoveX { get; private set; }
        public float Speed { get; private set;}

        public MovingState(Func<FighterRuntime,float> getMoveX, Func<FighterRuntime,float> getSpeed)
        {
            _getMoveX = getMoveX;
            _getSpeed = getSpeed;
        }

        public void Enter(FighterRuntime runtime)
        {
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