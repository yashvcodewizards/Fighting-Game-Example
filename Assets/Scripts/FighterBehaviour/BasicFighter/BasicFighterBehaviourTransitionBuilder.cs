using System;
using System.Collections.Generic;
using FightTest.StateMachine;
using FightTest.States;
using UnityEngine;

namespace FighterBehaviour.FighterBehaviours
{
    [CreateAssetMenu(menuName = "FightTest/FighterBehaviour/TransitionBuilder/Basic Fighter")]
    public class BasicFighterBehaviourTransitionBuilder : FighterBehaviourTransitionBuilder
    {
        public override bool CanBuildFrom(FighterBehaviourData data)
        {
            return data != null && data is BasicFighterBehaviourData;
        }

        public override Dictionary<IState, List<ITransition>> BuildTransitions(FighterRuntime runtime,
            FighterStateRegistry states)
        {
            var transitions = new Dictionary<IState, List<ITransition>>();

            var queries = runtime.Queries;

            var idle = states.Get(BasicFighterStateKeys.Idle);
            var walk = states.Get(BasicFighterStateKeys.Walk);
            var jumpRise = states.Get(BasicFighterStateKeys.JumpRise);
            var airborne = states.Get(BasicFighterStateKeys.Airborne);
            var hitStun = states.Get<HitStunState>(BasicFighterStateKeys.HitStun);
            var lightAttack = states.Get<AttackState>(BasicFighterStateKeys.LightAttack);
            var special1 = states.Get(BasicFighterStateKeys.Special1);

            // Root transitions
            RegisterCanJumpTransition(idle);
            RegisterCanJumpTransition(walk);

            RegisterLandingTransition(jumpRise, idle);
            RegisterLandingTransition(airborne, idle);

            // Ground transitions
            RegisterTransitions(
                idle,
                new HitReactionTransition(() => queries.IsPendingHit(), () => hitStun, HitReactionType.Hit),
                new Transition(() => queries.CanWalkFromIdle(), () => walk),
                new Transition(() => queries.IsTryingLightAttack(), () => lightAttack)
            );

            RegisterTransitions(
                walk,
                new HitReactionTransition(() => queries.IsPendingHit(), () => hitStun, HitReactionType.Hit),
                new Transition(() => queries.IsNeutral() && !queries.IsDucking(), () => idle),
                new Transition(() => queries.IsTryingLightAttack(), () => lightAttack)
            );

            // Air transitions
            RegisterTransitions(
                jumpRise,
                new Transition(() => queries.IsFalling(), () => airborne)
            );

            RegisterTransitions(
                hitStun,
                new Transition(() => queries.IsStateFinished(), () => idle)
            );

            RegisterTransitions(
                lightAttack,
                new Transition(() => queries.IsPendingHit(), () => hitStun),
                new Transition(() => queries.IsStateFinished(), () => idle)
            );
            
            RegisterSpecialTransition(special1, queries.IsTryingSpecial1);

            return transitions;

            void RegisterCanJumpTransition(IState state)
            {
                RegisterTransitions(
                    state,
                    new Transition(
                        () => queries.CanJumpFromGround(),
                        () => jumpRise)
                );
            }

            void RegisterLandingTransition(IState state, IState landState)
            {
                RegisterTransitions(
                    state,
                    new Transition(() => queries.IsLanding(), () => landState)
                );
            }

            void RegisterTransitions(IState state, params ITransition[] stateTransitions)
            {
                if (!transitions.ContainsKey(state))
                {
                    transitions[state] = new List<ITransition>();
                }

                transitions[state].AddRange(stateTransitions);
            }
            
            void RegisterSpecialTransition(IState specialState, Func<bool> inputCondition)
            {
                if (specialState == null)
                {
                    return;
                }

                RegisterTransitions(
                    idle,
                    new Transition(inputCondition, () => specialState)
                );

                RegisterTransitions(
                    walk,
                    new Transition(inputCondition, () => specialState)
                );

                RegisterTransitions(
                    specialState,
                    new Transition(
                        () => runtime.Services.StateFrameTimer.IsFinished,
                        () => idle
                    )
                );
            }
        }
    }
}