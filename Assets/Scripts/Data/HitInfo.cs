using FighterBehaviour;
using FightTest.Data;
using UnityEngine;

namespace Data
{
    public readonly struct HitInfo
    {
        public readonly FighterRuntime Attacker;
        public readonly AttackData AttackData;
        public readonly Vector2 Direction;

        public HitInfo(FighterRuntime attacker, AttackData attackData, Vector2 direction)
        {
            Attacker = attacker;
            AttackData = attackData;
            Direction = direction;
        }
    }
}