using UnityEngine;

namespace FighterBehaviour
{
    /// <summary>
    /// </summary>
    [CreateAssetMenu(menuName = "FightTest/FighterBehaviour/Definition")]
    public sealed class FighterBehaviourDefinition : ScriptableObject
    {
        [SerializeField] private FighterBehaviourData _behaviourData;
        [SerializeField] private FighterBehaviourTransitionBuilder _transitionBuilder;

        public FighterBehaviourPackage Build(FighterRuntime runtime)
        {
            if (!_transitionBuilder.CanBuildFrom(_behaviourData))
            {
                Debug.LogError($"{_transitionBuilder.name} cannot build transitions for {_behaviourData.name}");
                return null;
            }

            var states = _behaviourData.BuildStates(runtime);
            var transitions = _transitionBuilder.BuildTransitions(runtime, states);

            return new FighterBehaviourPackage(states.InitialState, transitions, states);
        }

        public void Initialize(FighterRuntime runtime)
        {
            _behaviourData.Initialize(runtime);
        }
    }
}