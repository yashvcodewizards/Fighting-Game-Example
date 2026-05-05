using Data;
using FighterBehaviour;
using FightTest.StateMachine;
using UnityEngine;

namespace FightTest.States
{
    public sealed class HitStunState : IState
    {
        private readonly BoxProfile _boxProfile;
        private readonly HitStunTimer _timer;

        public HitStunState(BoxProfile boxProfile, HitStunTimer timer)
        {
            _boxProfile = boxProfile;
            _timer = timer;
        }

        public bool IsFinished => _timer.IsFinished;

        public void Enter(FighterRuntime runtime)
        {
            runtime.Services.HitBoxManager.ApplyProfile(_boxProfile);
            
            var pendingHit = runtime.Context.PendingHit;

            if (!pendingHit.HasValue)
            {
                _timer.Configure(0);
                return;
            }
            
            var hitInfo = pendingHit.Value;
            var data = hitInfo.AttackData;

            runtime.Context.PendingHit = null;

            /*runtime.Services.Health.TakeDamage(data.Damage);*/ // TODO Add damaging

            runtime.Services.Mover.AddForce(
                new Vector2(
                    hitInfo.Direction.x * data.Knockback.x,
                    data.Knockback.y
                )
            );

            _timer.Configure(data.EnemyHitStopFrames);
        }

        public void Tick(FighterRuntime runtime)
        {
            _timer.Tick();
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}