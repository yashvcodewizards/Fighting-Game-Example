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
        [SerializeField] private CharacterHealth _health;
        [SerializeField] private CharacterMover _mover;
        [SerializeField] private FacingSystem _facing;
        [SerializeField] private LayerMask _hitLayer;
        [SerializeField] private GroundDetector _groundDetector;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private HitDetector _hitDetector;
        [SerializeField] private HitHandler _hitHandler;
        [SerializeField] private ColliderManager _colliderManager;
        [SerializeField] private FighterPresentation _fighterPresentation;
        
        private IInputProvider _inputProvider => _inputProviderBehaviour as IInputProvider;
        
        private StateTimer _stateTimer;
        private StateMachine.StateMachine _root;
        private FighterServices _services;
        private FighterBehaviourContext _context;
        private FighterRuntime _runtime;
        
        private bool _isInitialized;
        
        public bool IsInitialized => _isInitialized;
        public FighterRuntime Runtime => _runtime;
        
        private void Awake()
        {
            if (_inputProviderBehaviour != null && _inputProvider == null)
            {
                Debug.LogError($"{name} input provider does not implement IInputProvider.");
            }
            
            BuildRuntime();
        }
        
        private void Start()
        {
            if (_fighterDefinition != null)
            {
                InitializeFighter(_fighterDefinition);
            }
        }
        
        private void FixedUpdate()
        {
            if (!_isInitialized)
            {
                return;
            }
            
            _context.Frame = _inputProvider?.GetFrame() ?? default;
            _root.Tick();
        }
        
        private void BuildRuntime()
        {
            _stateTimer = new StateTimer();
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
                _stateTimer,
                _colliderManager,
                _hitDetector,
                _hitHandler,
                _fighterPresentation
            );

            _context = new FighterBehaviourContext();
            _runtime = new FighterRuntime(_services, _context);
        }
        
        public void InitializeFighter(FighterBehaviourDefinition fighterDefinition)
        {
            if (fighterDefinition == null)
            {
                Debug.LogError($"{name} cannot initialize with a null FighterBehaviourDefinition.");
                return;
            }
            
            if (_isInitialized)
            {
                ResetCurrentFighter();
            }

            _fighterDefinition = fighterDefinition;
            _fighterDefinition.Initialize(_runtime);

            var package = _fighterDefinition.Build(_runtime);
            if (package == null)
            {
                Debug.LogError($"{name} failed to build FighterBehaviourPackage.");
                return;
            }

            _root.Init(package, _runtime);
            _isInitialized = true;
        }
        
        private void ResetCurrentFighter()
        {
            _root.StopCurrentState();
            _colliderManager.ClearAll();
            _context.Reset();

            _isInitialized = false;
        }
    }
}