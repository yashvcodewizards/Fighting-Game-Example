using FighterBehaviour;
using FightTest.StateMachine;

namespace FightTest.States.FoitingueStates
{
    public class JumpState: IState
    {
        private readonly float _jumpForce;
        
        public JumpState(float jumpForce)
        {
            _jumpForce = jumpForce;
        }

        public void Enter(FighterRuntime runtime)
        {
            if (runtime.Context.SuppressNextJump)
            {
                runtime.Context.SuppressNextJump = false;
            }

            runtime.Services.Mover.Jump(_jumpForce, runtime.Context.PendingJumpDirectionX);
            
        }

        public void Tick(FighterRuntime runtime)
        {
        }

        public void Exit(FighterRuntime runtime)
        {
        }

        // TODO replace this usage later with 'context.SuppressNextJump = true;'
        /*public void ConfigureAsLaunched()
        {
            _suppressJump = true;
        }*/
    }
}