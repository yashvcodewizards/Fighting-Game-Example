using FightTest.States;

namespace FighterBehaviour
{
    public class FighterQueries
    {
        private readonly FighterServices _services;
        private readonly FighterBehaviourContext _context;

        public FighterQueries(FighterServices services, FighterBehaviourContext context)
        {
            _services = services;
            _context = context;
        }

        public bool IsGrounded()
        {
            return _services.GroundDetector && _services.GroundDetector.IsGrounded;
        }

        public bool IsWalkingBack()
        {
            return _context.Frame.MoveX * _services.Facing.Sign < 0f;
        }

        public bool IsMovingForward()
        {
            return _context.Frame.MoveX * _services.Facing.Sign > 0f;
        }
        
        public bool IsAirborne()
        {
            return _services.Root.CurrentState is AirbornState;
            //return _services.Root.CurrentState == airborn;
        }
        
        public bool IsGroundSubstateAttack() // for now checek against attack, dunno if in air
        {
            var state = _services.Root.CurrentState;
            return state is AttackState;
            /*return state == lightAttack
                   || state == heavyAttack
                   || state == throwAttack
                   || state == crouchLightAttack
                   || state == crouchHeavyAttack;*/
        }

        public bool IsHitStunned()
        {
            var state = _services.Root.CurrentState;
            return state is HitStunState;
            //return state == hitStun;
        }
    }
}