using FighterBehaviour;
using FightTest.StateMachine;

namespace FightTest.States.FoitingueStates
{
    public class JumpState: IState
    {
        private readonly float _jumpForce;
        private readonly float _horizontalSpeed;
        private readonly string _animationLabel;
        
        public JumpState(float jumpForce, float horizontalSpeed, string animationLabel = null)
        {
            _jumpForce = jumpForce;
            _horizontalSpeed = horizontalSpeed;
            _animationLabel = animationLabel;
        }

        public void Enter(FighterRuntime runtime)
        {
            var directionX = runtime.Context.Frame.MoveX * _horizontalSpeed;

            runtime.Services.Mover.Jump(_jumpForce, directionX);
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