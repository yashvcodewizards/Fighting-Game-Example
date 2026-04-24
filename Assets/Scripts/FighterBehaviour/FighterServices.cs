using FightTest.Data;
using FightTest.StateMachine;
using FightTest.States;
using FightTest.Systems;
using UnityEngine;

namespace FighterBehaviour
{
    public sealed class FighterServices
    {
        public MonoBehaviour InputProviderBehaviour { get; }
        public CharacterHealth Health{ get; }

        public CharacterMover Mover { get; }
        public FacingSystem Facing { get; }
        public LayerMask HitLayer { get; }
        public GroundDetector GroundDetector { get; }
        public Rigidbody2D Rb { get; }
        public StateMachine Root { get; }
        public GameObject Self { get; }
        public HitStunTimer HitStunTimer { get; }

        public FighterServices(
            MonoBehaviour inputProviderBehaviour,
            CharacterHealth health,
            CharacterMover mover,
            FacingSystem facing,
            LayerMask hitLayer,
            GroundDetector groundDetector,
            Rigidbody2D rb,
            StateMachine root,
            GameObject self,
            HitStunTimer hitStunTimer)
        {
            InputProviderBehaviour = inputProviderBehaviour;
            Health = health;
            Mover = mover;
            Facing = facing;
            HitLayer = hitLayer;
            GroundDetector = groundDetector;
            Rb = rb;
            Root = root;
            Self = self;
            HitStunTimer = hitStunTimer;
        }
    }
}