using FighterBehaviour;
using FightTest.Input;
using FightTest.States;
using FightTest.Systems;
using UnityEngine;

namespace FightTest.Controllers
{
    public class FoitingueController : MonoBehaviour
    {
        [SerializeField] private FighterBehaviourDefinition _fighterDefinition;

        [SerializeField] private MonoBehaviour _inputProviderBehaviour;
        private IInputProvider _inputProvider => _inputProviderBehaviour as IInputProvider;
        [SerializeField] private CharacterHealth _health;
        [SerializeField] private CharacterMover _mover;
        [SerializeField] private FacingSystem _facing;
        [SerializeField] private LayerMask _hitLayer;
        [SerializeField] private GroundDetector _groundDetector;
        [SerializeField] private Rigidbody2D _rb;
        
        private HitStunTimer _hitStunTimer;
        private StateMachine.StateMachine _root;
        private FighterServices _services;
        private FighterBehaviourContext _context;
        private FighterQueries _queries;

        private void Awake()
        {
            _hitStunTimer = new HitStunTimer();
            _root = new StateMachine.StateMachine();
            
            _services = new FighterServices(
                _inputProviderBehaviour,
                _health,
                _mover,
                _facing,
                _hitLayer,
                _groundDetector,
                _rb,
                _root,
                gameObject,
                _hitStunTimer,
                _queries
            );

            _context = new FighterBehaviourContext();

            _fighterDefinition.Initialize(_services);
            var package = _fighterDefinition.Build(_services, _context);
            _root.Init(package);
        }

        private void FixedUpdate()
        {
            _context.Frame = _inputProvider?.GetFrame() ?? default;

            _root.Tick();
        }
    }
}