using FightTest.Systems;
using UnityEngine;

namespace FightTest.Data
{
    [CreateAssetMenu(menuName = "FightTest/AttackData")]
    public class AttackData : ScriptableObject
    {
        [Header("Hit Timing & Boxes")]
        public ColliderTimeline ColliderTimeline;
        
        // TODO Add any unique responses eg. Different enemy stun frames if they are blocked etc

        [Header("Movement (Self)")]
        public Vector2 LungeForce;
        public int LungeFrame;

        [Header("Damage")]
        public int Damage = 5;
        
        [Header("Hit Reaction (Enemy)")]
        public int EnemyHitStunFrames = 4;
        
        [Header("KnockBack (Enemy)")]
        public Vector2 Knockback;
        public bool KnocksDown;
    }
}