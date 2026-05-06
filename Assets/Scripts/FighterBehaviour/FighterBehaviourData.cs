using UnityEngine;

namespace FighterBehaviour
{
    public abstract class FighterBehaviourData : ScriptableObject
    {
        public abstract FighterStateRegistry BuildStates(FighterRuntime runtime);

        public abstract void Initialize(FighterRuntime runtime);
    }
}