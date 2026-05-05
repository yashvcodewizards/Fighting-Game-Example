using UnityEngine;

namespace FighterBehaviour
{
    /// <summary>
    /// Base asset for defining a fighter's behaviour graph.
    /// Implementations create the fighter's states, transitions, and initial state,
    /// then return them as a behaviour package.
    /// </summary>
    public abstract class FighterBehaviourDefinition: ScriptableObject
    {
        public abstract FighterBehaviourPackage Build(FighterRuntime runtime);

        public abstract void Initialize(FighterRuntime runtime);
    }
}