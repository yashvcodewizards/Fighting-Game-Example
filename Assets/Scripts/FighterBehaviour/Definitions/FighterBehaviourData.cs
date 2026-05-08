using UnityEngine;

namespace FighterBehaviour
{
    public abstract class FighterBehaviourData : ScriptableObject
    {
        public abstract StateRegistry BuildStates(FighterRuntime runtime);

        public abstract void Initialize(FighterRuntime runtime);
    }
}