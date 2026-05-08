using Data;
using FighterBehaviour;
using FightTest.StateMachine;
using UnityEngine;

namespace FightTest.States
{
    public sealed class HitStunState : IState
    {
        private readonly BoxProfile _boxProfile;
        private readonly string _animationLabel;

        public HitStunState(BoxProfile boxProfile, string animationLabel = null)
        {
            _boxProfile = boxProfile;
            _animationLabel = animationLabel;
        }

        public void Enter(FighterRuntime runtime)
        {
            runtime.Services.HitBoxManager.ApplyProfile(_boxProfile);
            
            var pendingHit = runtime.Context.PendingHit;

            if (!pendingHit.HasValue)
            {
                runtime.Context.PendingHit = null;
                runtime.Services.StateFrameTimer.Start(0);
                return;
            }
            
            var hitInfo = pendingHit.Value;
            var data = hitInfo.AttackData;

            runtime.Context.PendingHit = null;

            runtime.Services.Health.TakeDamage(data.Damage);

            runtime.Services.Mover.AddForce(
                new Vector2(
                    hitInfo.Direction.x * data.Knockback.x,
                    data.Knockback.y
                )
            );
            
            runtime.Services.Presentation.Play(_animationLabel);
            runtime.Services.StateFrameTimer.Start(data.EnemyHitStunFrames);
        }

        public void Tick(FighterRuntime runtime)
        {
            runtime.Services.StateFrameTimer.Tick();
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}