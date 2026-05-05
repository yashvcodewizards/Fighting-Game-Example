using Data;
using FighterBehaviour;
using UnityEngine;

namespace FightTest.Systems
{
    public class HitHandler : MonoBehaviour, IHittable
    {
        private FighterRuntime _runtime;

        public void Initialize(FighterRuntime runtime)
        {
            _runtime = runtime;
        }

        public void ReceiveHit(HitInfo hitInfo)
        {
            if (_runtime == null)
            {
                Debug.LogWarning($"{name} HitHandler has no runtime.");
                return;
            }

            _runtime.Context.PendingHit = hitInfo;
            
            /*if (_controller.QueryIsInvulnerable)
            {
                return;
            }

            if (data.KnocksDown)
            {
                ReceiveThrow(data);
                return;
            }


            if (_controller.QueryIsAirborne)
            {
                _controller.OnAirHit();
                TakeDamage(data);
                return;
            }

            if (_controller.QueryIsWalkingBack && CanBlock(data))
            {
                if (_controller.QueryIsInCrouchState)
                {
                    _controller.OnCrouchBlock(data.EnemyBlockStunFrames);
                }
                else
                {
                    _controller.OnGroundBlock(data.EnemyBlockStunFrames);
                }

                _controller.Mover.AddForce(
                    new Vector2(-_controller.Facing.Sign * data.BlockKnockback.x, data.BlockKnockback.y)
                );
            }
            else
            {
                _controller.OnGroundHit();
                TakeDamage(data);
            }*/
        }


        /*public void ReceiveThrow(AttackData data)
        {
            /*if (_controller.QueryIsInvulnerable)
            {
                return;
            }

            _controller.Health.TakeDamage(data.Damage);
            _controller.Mover.AddForce(new Vector2(-_controller.Facing.Sign * data.Knockback.x, data.Knockback.y));

            var shouldLaunch = _controller.QueryIsAirborne || data.Knockback.y > 0;
            _controller.OnThrowLaunch(shouldLaunch);

            if (!_controller.QueryIsAirborne)
            {
                _controller.OnGroundKnockdown();
            }#1#
        }

        private void TakeDamage(AttackData data)
        {
            /*_controller.Mover.AddForce(new Vector2(-_controller.Facing.Sign * data.Knockback.x, data.Knockback.y));
            _controller.Health.TakeDamage(data.Damage);
            _controller.HitStunTimer.Configure(data.EnemyHitStopFrames);#1#
        }

        private bool CanBlock(AttackData data)
        {
            /*if (_controller.QueryIsInWalkState)
            {
                return data.Height == AttackHeight.Mid || data.Height == AttackHeight.Air;
            }

            if (_controller.QueryIsInCrouchState)
            {
                return data.Height == AttackHeight.Mid || data.Height == AttackHeight.Low;
            }
#1#
            return false;
        }*/
    }
}