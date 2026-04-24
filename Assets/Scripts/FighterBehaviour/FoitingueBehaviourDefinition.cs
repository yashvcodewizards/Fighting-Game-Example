using System.Collections.Generic;
using FightTest.Data;
using FightTest.StateMachine;
using FightTest.States;
using FightTest.Systems;
using UnityEngine;

namespace FighterBehaviour
{
    [CreateAssetMenu(menuName = "FightTest/FighterBehaviour/FoitingueBehaviour")]
    public class FoitingueBehaviourDefinition : FighterBehaviourDefinition
    {
        #region colliders test

        [SerializeField] private ColliderSet _idleColliders;

        [SerializeField] private ColliderSet _walkColliders;
        [SerializeField] private ColliderSet _sprintColliders;
        [SerializeField] private ColliderSet _dashColliders;
        [SerializeField] private ColliderSet _crouchColliders;
        [SerializeField] private ColliderSet _crouchWalkColliders;
        [SerializeField] private ColliderSet _blockColliders;
        [SerializeField] private ColliderSet _crouchBlockColliders;
        [SerializeField] private ColliderSet _hitStunColliders;
        [SerializeField] private ColliderSet _airHitStunColliders;
        [SerializeField] private ColliderSet _knockedDownColliders;
        [SerializeField] private ColliderSet _airKnockedDownColliders;
        [SerializeField] private ColliderSet _jumpRiseColliders;
        [SerializeField] private ColliderSet _airborneColliders;
        [SerializeField] private ColliderSet _lightColliders;
        [SerializeField] private ColliderSet _heavyColliders;
        [SerializeField] private ColliderSet _throwColliders;
        [SerializeField] private ColliderSet _crouchLightColliders;
        [SerializeField] private ColliderSet _crouchHeavyColliders;
        [SerializeField] private ColliderSet _airLightColliders;
        [SerializeField] private ColliderSet _airHeavyColliders;
        [SerializeField] private ColliderSet _airThrowColliders;
        

        #endregion
        
        [Header("Movement")]
        public float MoveSpeed = 4f;
        public float WalkBackSpeed = 2.5f;
        public float SprintSpeed = 8f;
        public float BackDashSpeed = 10f;
        public float BackDashDuration = 0.2f;
        public float JumpForce = 10f;

        [Header("Health")]
        public int MaxHealth = 100;
        
        [Header("Attack Data")]
        public AttackData ThrowAttack;
        public AttackData LightAttack;
        public AttackData HeavyAttack;
        public AttackData CrouchLightAttack;
        public AttackData CrouchHeavyAttack;
        public AttackData AirLightAttack;
        public AttackData AirHeavyAttack;
        
        public override FighterBehaviourPackage Build(FighterRuntime runtime)
        {
            var services = runtime.Services;
            var context = runtime.Context;
            var queries = runtime.Queries;
            
            // Simple states
            var idle = new SimpleState(_idleColliders);
            var crouch = new SimpleState(_crouchColliders);
            var jumpRise = new SimpleState(_jumpRiseColliders);
            var airborne = new SimpleState(_airborneColliders);

            // Movement states
            var walk = new MovingState(
                services.Mover,
                _walkColliders,
                () => context.Frame.MoveX,
                () => queries.IsWalkingBack()
                    ? WalkBackSpeed
                    : MoveSpeed
            );
            var sprint = new MovingState(
                services.Mover,
                _sprintColliders,
                () => context.Frame.MoveX,
                () => SprintSpeed
            );
            var crouchWalk = new MovingState(
                services.Mover,
                _crouchWalkColliders,
                () => queries.IsWalkingBack() ? 0f : context.Frame.MoveX,
                () => MoveSpeed
            );

            // Defensive / reaction states
            var dash = new DashState(
                services.Mover,
                BackDashSpeed,
                BackDashDuration,
                _dashColliders);

            var block = new BlockStunState(_blockColliders);
            var crouchBlock = new BlockStunState(_crouchBlockColliders);
            var hitStun = new HitStunState(_hitStunColliders, services.HitStunTimer);
            var airHitStun = new HitStunState(_airHitStunColliders, services.HitStunTimer);
            var knockedDown = new KnockedDownState(_knockedDownColliders, 75);
            var airKnockedDown = new KnockedDownState(_airKnockedDownColliders, 0);

            // Attack states
            var throwAttack = new ThrowAttackState(
                ThrowAttack,
                _throwColliders,
                services.HitLayer,
                services.Self);

            var airThrowAttack = new ThrowAttackState(
                ThrowAttack,
                _airThrowColliders,
                services.HitLayer,
                services.Self);

            var lightAttack = new AttackState(
                LightAttack,
                _lightColliders,
                services.HitLayer,
                services.Facing,
                services.Self,
                "LightAttack",
                services.Mover);

            var heavyAttack = new AttackState(
                HeavyAttack,
                _heavyColliders,
                services.HitLayer,
                services.Facing,
                services.Self,
                "HeavyAttack",
                services.Mover);

            var crouchLightAttack = new AttackState(
                CrouchLightAttack,
                _crouchLightColliders,
                services.HitLayer,
                services.Facing,
                services.Self,
                "CrouchLight",
                services.Mover);

            var crouchHeavyAttack = new AttackState(
                CrouchHeavyAttack,
                _crouchHeavyColliders,
                services.HitLayer,
                services.Facing,
                services.Self,
                "CrouchHeavy",
                services.Mover);

            var airLightAttack = new AttackState(
                AirLightAttack,
                _airLightColliders,
                services.HitLayer,
                services.Facing,
                services.Self,
                "AirLight",
                services.Mover);

            var airHeavyAttack = new AttackState(
                AirHeavyAttack,
                _airHeavyColliders,
                services.HitLayer,
                services.Facing,
                services.Self,
                "AirHeavy",
                services.Mover);

            // Root states
            var ground = new GroundState(idle);
            var airborn = new AirbornState(jumpRise, services.Mover, JumpForce);

            var transitions = new Dictionary<IState, List<ITransition>>();

            void RegisterTransitions(IState state, params ITransition[] stateTransitions)
            {
                if (!transitions.ContainsKey(state))
                {
                    transitions[state] = new List<ITransition>();
                }

                transitions[state].AddRange(stateTransitions);
            }

            // Root transitions
            RegisterTransitions(
                ground,
                new Transition(
                    () => queries.IsGrounded() && context.Frame.Jump && !queries.IsGroundSubstateAttack() && !queries.IsHitStunned(),
                    () =>
                    {
                        airborn.Configure(context.Frame.MoveX * MoveSpeed);
                        return airborn;
                    })
            );

            RegisterTransitions(
                airborn,
                new Transition(
                    () => queries.IsGrounded() && services.Rb.velocity.y <= 0.1f,
                    () =>
                    {
                        var airSubState = airborn.SubMachine.CurrentState;

                        if (airSubState == airKnockedDown)
                        {
                            ground.SubMachine.ChangeState(knockedDown);
                        }
                        else if (airSubState == airHitStun)
                        {
                            ground.SubMachine.ChangeState(hitStun);
                        }
                        else
                        {
                            ground.SubMachine.ChangeState(idle);
                        }

                        return ground;
                    })
            );

            var groundSm = ground.SubMachine;

            // Ground transitions
            RegisterTransitions(
                idle,
                new Transition(() => context.Frame.BackDash && queries.IsWalkingBack(), () => dash),
                new Transition(
                    () =>
                    {
                        var result = context.Frame.MoveX != 0f &&
                                     context.Frame is { Duck: false, LightAttack: false, HeavyAttack: false };

                        Debug.Log($"Idle -> Walk check: MoveX={context.Frame.MoveX}, result={result}");
                        return result;
                    },
                    () => walk),
                new Transition(() => context.Frame.Duck, () => crouch),
                new Transition(() => context.Frame.LightAttack, () => lightAttack),
                new Transition(() => context.Frame.HeavyAttack, () => heavyAttack),
                new Transition(() => context.Frame.Throw, () => throwAttack)
            );

            RegisterTransitions(
                walk,
                new Transition(() => context.Frame.BackDash && queries.IsWalkingBack(), () => dash),
                new Transition(() => context.Frame.Sprint && !context.Frame.Duck && queries.IsMovingForward(), () => sprint),
                new Transition(() => context.Frame is { MoveX: 0f, Duck: false }, () => idle),
                new Transition(() => context.Frame.Duck && (context.Frame.MoveX == 0f || queries.IsWalkingBack()), () => crouch),
                new Transition(() => context.Frame.Duck && queries.IsMovingForward(), () => crouchWalk),
                new Transition(() => context.Frame.LightAttack, () => lightAttack),
                new Transition(() => context.Frame.HeavyAttack, () => heavyAttack),
                new Transition(() => context.Frame.Throw, () => throwAttack)
            );

            RegisterTransitions(
                sprint,
                new Transition(() => !queries.IsMovingForward() || !context.Frame.Sprint, () => idle),
                new Transition(() => context.Frame.Duck, () => crouch),
                new Transition(() => context.Frame.LightAttack, () => lightAttack),
                new Transition(() => context.Frame.HeavyAttack, () => heavyAttack),
                new Transition(() => context.Frame.Throw, () => throwAttack)
            );

            RegisterTransitions(
                crouch,
                new Transition(() => context.Frame is { Duck: false, MoveX: 0f }, () => idle),
                new Transition(() => !context.Frame.Duck && context.Frame.MoveX != 0f, () => walk),
                new Transition(() => context.Frame.Duck && queries.IsMovingForward(), () => crouchWalk),
                new Transition(() => context.Frame.LightAttack, () => crouchLightAttack),
                new Transition(() => context.Frame.HeavyAttack, () => crouchHeavyAttack)
            );

            RegisterTransitions(
                crouchWalk,
                new Transition(() => !context.Frame.Duck && context.Frame.MoveX == 0f, () => idle),
                new Transition(() => !context.Frame.Duck && context.Frame.MoveX != 0f, () => walk),
                new Transition(() => context.Frame.Duck && (context.Frame.MoveX == 0f || queries.IsWalkingBack()), () => crouch),
                new Transition(() => context.Frame.LightAttack, () => crouchLightAttack),
                new Transition(() => context.Frame.HeavyAttack, () => crouchHeavyAttack)
            );

            RegisterTransitions(
                dash,
                new Transition(() => dash.IsFinished, () => idle)
            );

            RegisterTransitions(
                block,
                new Transition(() => block.IsFinished, () => idle)
            );

            RegisterTransitions(
                crouchBlock,
                new Transition(() => crouchBlock.IsFinished, () => crouch)
            );

            RegisterTransitions(
                lightAttack,
                new Transition(() => lightAttack.IsFinished, () => idle)
            );

            RegisterTransitions(
                heavyAttack,
                new Transition(() => heavyAttack.IsFinished, () => idle)
            );

            RegisterTransitions(
                throwAttack,
                new Transition(() => throwAttack.IsFinished, () => idle)
            );

            RegisterTransitions(
                crouchLightAttack,
                new Transition(() => crouchLightAttack.IsFinished, () => crouch)
            );

            RegisterTransitions(
                crouchHeavyAttack,
                new Transition(() => crouchHeavyAttack.IsFinished, () => crouch)
            );

            RegisterTransitions(
                hitStun,
                new Transition(() => hitStun.IsFinished, () => idle)
            );

            RegisterTransitions(
                knockedDown,
                new Transition(() => knockedDown.IsFinished, () => idle)
            );

            var airSm = airborn.SubMachine;

            // Air transitions
            RegisterTransitions(
                jumpRise,
                new Transition(() => services.Rb.velocity.y <= 0f, () => airborne),
                new Transition(() => context.Frame.LightAttack, () => airLightAttack),
                new Transition(() => context.Frame.HeavyAttack, () => airHeavyAttack),
                new Transition(() => context.Frame.Throw, () => airThrowAttack)
            );

            RegisterTransitions(
                airborne,
                new Transition(() => context.Frame.LightAttack, () => airLightAttack),
                new Transition(() => context.Frame.HeavyAttack, () => airHeavyAttack),
                new Transition(() => context.Frame.Throw, () => airThrowAttack)
            );

            RegisterTransitions(
                airLightAttack,
                new Transition(() => airLightAttack.IsFinished, () => airborne)
            );

            RegisterTransitions(
                airHeavyAttack,
                new Transition(() => airHeavyAttack.IsFinished, () => airborne)
            );

            RegisterTransitions(
                airThrowAttack,
                new Transition(() => airThrowAttack.IsFinished, () => airborne)
            );

            RegisterTransitions(
                airHitStun,
                new Transition(() => airHitStun.IsFinished, () => airborne)
            );

            return new FighterBehaviourPackage(idle, transitions);
        }

        public override void Initialize(FighterServices services)
        {
            services.Health.Init(MaxHealth);
        }
    }
}