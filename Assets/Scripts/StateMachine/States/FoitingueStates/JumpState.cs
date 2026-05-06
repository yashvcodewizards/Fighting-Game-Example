using FighterBehaviour;
using FightTest.StateMachine;

namespace FightTest.States.FoitingueStates
{
    public class JumpState: IState
    {
        private readonly float _jumpForce;
        private readonly float _horizontalSpeed;
        
        public JumpState(float jumpForce, float horizontalSpeed)
        {
            _jumpForce = jumpForce;
            _horizontalSpeed = horizontalSpeed;
        }

        public void Enter(FighterRuntime runtime)
        {
            var directionX = runtime.Context.Frame.MoveX * _horizontalSpeed;

            runtime.Services.Mover.Jump(_jumpForce, directionX);
        }

        public void Tick(FighterRuntime runtime)
        {
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}