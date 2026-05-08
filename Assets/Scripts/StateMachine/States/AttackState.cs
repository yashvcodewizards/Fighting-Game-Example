using FighterBehaviour;
using FightTest.Data;
using FightTest.StateMachine;
using UnityEngine;

namespace FightTest.States
{
    public sealed class AttackState : IState
    {
        private readonly AttackData _data;
        private readonly string _animationLabel;

        private bool _hasLunged;

        public AttackState(
            AttackData data,
            string animationLabel)
        {
            _data = data;
            _animationLabel = animationLabel;
        }
        
        public void Enter(FighterRuntime runtime)
        {
            _hasLunged = false;
            
            var duration = _data.ColliderTimeline
                ? _data.ColliderTimeline.TotalFrames
                : 0;
            
            runtime.Services.HitDetector.BeginAttack();
            runtime.Services.Presentation.Play(_animationLabel);
            runtime.Services.StateTimer.Start(duration);
        }

        public void Tick(FighterRuntime runtime)
        {
            TryLunge(runtime);
            
            runtime.Services.HitBoxManager.ApplyTimelineFrame(_data.ColliderTimeline, runtime.Services.StateTimer.CurrentFrame);
            runtime.Services.HitDetector.TryHit(runtime, _data);
        }

        public void Exit(FighterRuntime runtime)
        {
            runtime.Services.HitBoxManager.ClearHitboxes();
        }

        private void TryLunge(FighterRuntime runtime)
        {
            if (_hasLunged)
            {
                return;
            }

            if (_data.LungeForce.magnitude <= 0f)
            {
                return;
            }

            if (runtime.Services.StateTimer.CurrentFrame < _data.LungeFrame)
            {
                return;
            }

            runtime.Services.Mover.AddForce(
                new Vector2(
                    runtime.Services.Facing.Sign * _data.LungeForce.x,
                    _data.LungeForce.y
                )
            );

            _hasLunged = true;
        }
    }
}