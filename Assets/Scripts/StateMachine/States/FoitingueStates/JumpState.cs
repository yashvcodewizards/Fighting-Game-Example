using FighterBehaviour;
using FightTest.StateMachine;
using FightTest.Systems;

namespace FightTest.States.FoitingueStates
{
    public class JumpState: IState
    {
        private readonly ColliderSet _colliders;
        private readonly float _jumpForce;
        private float _directionX;
        private bool _suppressJump;
        
        public JumpState(ColliderSet colliders, float jumpForce)
        {
            _colliders = colliders;
            _jumpForce = jumpForce;
        }

        public void Enter(FighterRuntime runtime)
        {
            _colliders?.EnableSet();
            
            if (!_suppressJump)
            {
                runtime.Services.Mover.Jump(_jumpForce, _directionX);
            }
        }

        public void Tick(FighterRuntime runtime)
        {
        }

        public void Exit(FighterRuntime runtime)
        {
            _colliders?.DisableSet();
        }
        
        public void Configure(float directionX)
        {
            _directionX = directionX;
            _suppressJump = false;
        }

        public void ConfigureAsLaunched()
        {
            _suppressJump = true;
        }
    }
}