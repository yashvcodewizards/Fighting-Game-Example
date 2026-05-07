using FightTest.Data;
using UnityEngine;

namespace Data
{
    public readonly struct HitInfo
    {
        // TODO public readonly int AttackerId;
        public readonly AttackData AttackData;
        public readonly Vector2 Direction;

        public HitInfo(AttackData attackData, Vector2 direction)
        {
            AttackData = attackData;
            Direction = direction;
        }
    }
}