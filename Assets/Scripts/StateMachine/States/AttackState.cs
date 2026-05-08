using FighterBehaviour;
using FightTest.Data;
using FightTest.StateMachine;
using UnityEngine;

namespace FightTest.States
{
    public sealed class AttackState : IState
    {
        private readonly AttackData _data;
        private readonly string _label;

        private bool _hasLunged;

        public AttackState(
            AttackData data,
            string label)
        {
            _data = data;
            _label = label;
        }
        
        public void Enter(FighterRuntime runtime)
        {
            _hasLunged = false;
            
            var duration = _data.BoxTimeline
                ? _data.BoxTimeline.TotalFrames
                : 0;
            
            runtime.Services.StateFrameTimer.Start(duration);
            runtime.Services.HitDetector.BeginAttack();
            
            // Later:
            // runtime.Services.Animation.Play(_label);
        }

        public void Tick(FighterRuntime runtime)
        {
            TryLunge(runtime);
            
            runtime.Services.HitBoxManager.ApplyTimelineFrame(_data.BoxTimeline, runtime.Services.StateFrameTimer.CurrentFrame);
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

            if (runtime.Services.StateFrameTimer.CurrentFrame < _data.LungeFrame)
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