using FightTest.StateMachine;

namespace FightTest.Systems
{
    public interface IHitReaction
    {
        HitReactionType ReactionType { get; }
    }
}