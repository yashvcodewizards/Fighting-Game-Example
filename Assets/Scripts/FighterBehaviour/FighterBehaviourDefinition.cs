using FightTest.Controllers;
using UnityEngine;

namespace FighterBehaviour
{
    public abstract class FighterBehaviourDefinition: ScriptableObject
    {
        public abstract FighterBehaviourPackage Build(FighterServices services, FighterBehaviourContext context);

        public abstract void Initialize(FighterServices services);
    }
}