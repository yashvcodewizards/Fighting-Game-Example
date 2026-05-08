using FightTest.StateMachine;
using FightTest.States;
using FightTest.States.FoitingueStates;
using FightTest.Systems;

namespace FighterBehaviour
{
    /// <summary>
    /// Read-only helper layer for common fighter checks.
    /// Provides derived queries such as grounded state, movement direction,
    /// and transition conditions without putting that logic in the controller.
    /// </summary>
    public class FighterQueries
    {
        private readonly FighterServices _services;
        private readonly FighterContext _context;

        public FighterQueries(FighterServices services, FighterContext context)
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
            return _services.Root.CurrentState is JumpState;
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
        
        public bool HasMovementInput()
        {
            return _context.Frame.MoveX != 0f;
        }

        public bool IsNeutral()
        {
            return _context.Frame.MoveX == 0f;
        }

        public bool IsDucking()
        {
            return _context.Frame.Duck;
        }

        public bool IsTryingToJump()
        {
            return _context.Frame.Jump;
        }

        public bool IsTryingToBackDash()
        {
            return _context.Frame.BackDash;
        }

        public bool IsTryingToSprint()
        {
            return _context.Frame.Sprint;
        }

        public bool IsTryingLightAttack()
        {
            return _context.Frame.LightAttack;
        }

        public bool IsTryingHeavyAttack()
        {
            return _context.Frame.HeavyAttack;
        } 
        
        public bool IsTryingSpecial1()
        {
            return _context.Frame.Special1;
        }

        public bool IsTryingThrow()
        {
            return _context.Frame.Throw;
        }

        public bool IsFalling()
        {
            return _services.Rb.velocity.y <= 0f;
        }

        public bool IsLanding()
        {
            return IsGrounded() && _services.Rb.velocity.y <= 0.1f;
        }

        public bool CanWalkFromIdle()
        {
            return HasMovementInput()
                   && !IsDucking()
                   && !IsTryingLightAttack()
                   && !IsTryingHeavyAttack();
        }

        public bool CanJumpFromGround()
        {
            return IsGrounded()
                   && IsTryingToJump()
                   && !IsGroundSubstateAttack()
                   && !IsHitStunned();
        }

        public bool IsPendingHit()
        {
            return _context.PendingHit.HasValue;
        }

        public bool IsStateFinished()
        {
            return _services.StateTimer.IsFinished && _services.StateTimer.DurationFrames > 0;
        }
        
        public HitReactionType PreviewHitReaction()
        {
            var transitions = _services.Root.GetTransitions(_services.Root.CurrentState);
            foreach (var transition in transitions)
            {
                if (!(transition is IHitReaction hitTransition))
                {
                    continue;
                }

                var next = transition.Evaluate();
                if (next != null)
                {
                    return hitTransition.ReactionType;
                }
            }

            return HitReactionType.Miss;
        }
    }
}