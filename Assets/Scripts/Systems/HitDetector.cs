using Data;
using FighterBehaviour;
using FightTest.Data;
using UnityEngine;

namespace FightTest.Systems
{
    public sealed class HitDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _hitLayer;

        private readonly Collider2D[] _overlapBuffer = new Collider2D[16];

        private bool _hasHitThisSwing;
        
        public void BeginAttack()
        {
            _hasHitThisSwing = false;
        }
        
        public void TryHit(FighterRuntime attacker, AttackData data)
        {
            if (_hasHitThisSwing)
            {
                return;
            }
            
            var filter = new ContactFilter2D();
            filter.SetLayerMask(_hitLayer);
            filter.useTriggers = true;

            foreach (var hitbox in attacker.Services.HitBoxManager.ActiveHitBoxes)
            {
                if (hitbox == null || !hitbox.enabled)
                {
                    continue;
                }
                
                var count = hitbox.OverlapCollider(filter, _overlapBuffer);
                for (var i = 0; i < count; i++)
                {
                    if (_overlapBuffer[i].transform.IsChildOf(attacker.Services.Self.transform))
                    {
                        continue;
                    }

                    var hittable = _overlapBuffer[i].GetComponentInParent<IHittable>();
                    if (hittable == null)
                    {
                        continue;
                    }
                    
                    var hitInfo = new HitInfo(
                        attacker,
                        data,
                        new Vector2(attacker.Services.Facing.Sign, 0f)
                    );

                    _hasHitThisSwing = true;
                    hittable.ReceiveHit(hitInfo);
                    return;
                }
                
            }
        }

    }
}