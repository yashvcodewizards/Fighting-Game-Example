using FightTest.Controllers;
using UnityEngine;

namespace FighterBehaviour
{
    public abstract class FighterBehaviourDefinition: ScriptableObject
    {
        public abstract FighterBehaviourPackage Build(FighterRuntime runtime);

        public abstract void Initialize(FighterServices services);
    }
}