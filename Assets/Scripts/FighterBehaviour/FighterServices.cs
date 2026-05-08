using FightTest.StateMachine;
using FightTest.States;
using FightTest.Systems;
using UnityEngine;

namespace FighterBehaviour
{
    /// <summary>
    /// Service container for runtime systems used by fighter states.
    /// Holds references to components that perform actions, such as movement,
    /// facing, health, ground detection, and animation-related systems.
    /// </summary>
    public sealed class FighterServices
    {
        public CharacterHealth Health{ get; }
        public CharacterMover Mover { get; }
        public FacingSystem Facing { get; }
        public LayerMask HitLayer { get; }
        public GroundDetector GroundDetector { get; }
        public Rigidbody2D Rb { get; }
        public StateMachine Root { get; }
        public GameObject Self { get; }
        public StateTimer StateTimer { get; }
        public HitBoxManager HitBoxManager { get; }
        public HitDetector HitDetector { get; }
        public HitHandler HitHandler { get; }
        
        public FighterPresentation Presentation { get; }

        public FighterServices(
            CharacterHealth health,
            CharacterMover mover,
            FacingSystem facing,
            LayerMask hitLayer,
            GroundDetector groundDetector,
            Rigidbody2D rb,
            StateMachine root,
            GameObject self,
            StateTimer stateTimer,
            HitBoxManager hitBoxManager,
            HitDetector hitDetector,
            HitHandler hitHandler,
            FighterPresentation presentation)
        {
            Health = health;
            Mover = mover;
            Facing = facing;
            HitLayer = hitLayer;
            GroundDetector = groundDetector;
            Rb = rb;
            Root = root;
            Self = self;
            StateTimer = stateTimer;
            HitBoxManager = hitBoxManager;
            HitDetector = hitDetector;
            HitHandler = hitHandler;
            Presentation = presentation;
        }
    }
}