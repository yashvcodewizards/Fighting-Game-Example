using Data;
using FightTest.Data;
using FightTest.States;
using FightTest.States.FoitingueStates;
using UnityEngine;

namespace FighterBehaviour.FighterBehaviours
{
    [CreateAssetMenu(menuName = "FightTest/FighterBehaviour/Data/Basic Fighter")]
    public class BasicFighterBehaviourData : FighterBehaviourData
    {
        [Header("Movement")] public float MoveSpeed = 4f;
        public float WalkBackSpeed = 2.5f;

        public float JumpForce = 10f;

        [Header("Health")] public int MaxHealth = 100;

        [Header("BoxData")] 
        public BoxProfile IdleBoxProfile;
        public BoxProfile CapturedProfile;
        public BoxProfile HitStunBoxProfile;

        [Header("Attack Data")] public AttackData LightAttack;
        
        [Header("Special States")]
        public SpecialStateAsset Special1;

        public override void Initialize(FighterRuntime runtime)
        {
            runtime.Services.Health.Init(MaxHealth);
            runtime.Services.HitHandler.Initialize(runtime);
        }

        public override FighterStateRegistry BuildStates(FighterRuntime runtime)
        {
            var states = new FighterStateRegistry();

            // Simple states
            var idle = new SimpleState(IdleBoxProfile);
            var jumpRise = new JumpState(JumpForce, MoveSpeed);
            var airborne = new SimpleState(IdleBoxProfile);

            // Movement states
            var walk = new MovingState(
                r => r.Context.Frame.MoveX,
                r => r.Queries.IsWalkingBack()
                    ? WalkBackSpeed
                    : MoveSpeed
            );

            // Defensive / reaction states
            var hitStun = new HitStunState(HitStunBoxProfile);
            var lightAttack = new AttackState(LightAttack, BasicFighterStateKeys.LightAttack);
            var special1 = Special1.CreateRuntimeState();
            var captured = new CapturedState(CapturedProfile);

            states.Add(BasicFighterStateKeys.Idle, idle);
            states.Add(BasicFighterStateKeys.JumpRise, jumpRise);
            states.Add(BasicFighterStateKeys.Airborne, airborne);
            states.Add(BasicFighterStateKeys.Walk, walk);
            states.Add(BasicFighterStateKeys.HitStun, hitStun);
            states.Add(BasicFighterStateKeys.LightAttack, lightAttack);
            states.Add(BasicFighterStateKeys.Special1, special1);
            states.Add(BasicFighterStateKeys.Captured, captured);

            states.SetInitial(BasicFighterStateKeys.Idle);

            return states;
        }
    }
}