using FighterBehaviour;
using FightTest.Input;
using FightTest.States;
using FightTest.Systems;
using UnityEngine;

namespace FightTest.Controllers
{
    public class DefaultFighterController : MonoBehaviour
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
        [SerializeField] private HitHandler _hitHandler;
        [SerializeField] private HitBoxManager _hitBoxManager;
        
        private HitStunTimer _hitStunTimer;
        private StateMachine.StateMachine _root;
        private FighterServices _services;
        private FighterBehaviourContext _context;
        private FighterRuntime _runtime;
        public FighterQueries Queries => _runtime.Queries;
        
        private void Awake()
        {
            _hitStunTimer = new HitStunTimer();
            _root = new StateMachine.StateMachine();
            
            _services = new FighterServices(
                _health,
                _mover,
                _facing,
                _hitLayer,
                _groundDetector,
                _rb,
                _root,
                gameObject,
                _hitStunTimer,
                _hitBoxManager
            );

            _context = new FighterBehaviourContext();
            _runtime = new FighterRuntime(_services, _context);

            _fighterDefinition.Initialize(_services);
            
            var package = _fighterDefinition.Build(_runtime);
            _root.Init(package, _runtime);
        }

        private void FixedUpdate()
        {
            _context.Frame = _inputProvider?.GetFrame() ?? default;

            _root.Tick();
        }
        
        /*public void ChangeBehaviour(FighterBehaviourDefinition newDefinition)
        {
            _stateMachine.StopCurrentState();

            _presentation.Clear();
            _hitboxes.Clear();

            _definition = newDefinition;
            _definition.Initialize(_runtime.Services);

            var package = _definition.Build(_runtime);

            _runtime.Context.ResetForNewBehaviour();

            _stateMachine.Init(package, _runtime);
        }*/
    }
}