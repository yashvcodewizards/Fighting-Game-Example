using Data;
using FightTest.Data;

namespace FighterBehaviour
{
    /// <summary>
    /// Runtime data shared by fighter states and transitions.
    /// Stores values that change during play, such as the current input frame.
    /// </summary>
    public sealed class FighterBehaviourContext
    {
        public InputFrame Frame;
        
        //Jump
        public float PendingJumpDirectionX;
        public bool SuppressNextJump;
        
        //Attack
        public bool AttackHasHitThisSwing;
        public float AttackTimer;
        public bool AttackWasInActive;
    }
}