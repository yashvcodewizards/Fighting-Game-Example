using System;
using FightTest.Systems;

namespace FightTest.StateMachine
{
    public sealed class HitReactionTransition : ITransition, IHitReaction
    {
        private readonly Func<bool> _condition;
        private readonly Func<IState> _target;

        public HitReactionType ReactionType { get; }

        public HitReactionTransition(
            Func<bool> condition,
            Func<IState> target,
            HitReactionType reactionType)
        {
            _condition = condition;
            _target = target;
            ReactionType = reactionType;
        }

        public IState Evaluate()
        {
            return _condition() ? _target() : null;
        }
    }
}