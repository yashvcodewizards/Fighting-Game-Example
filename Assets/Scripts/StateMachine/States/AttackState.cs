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

        private int _currentFrame;
        private bool _hasLunged;

        public AttackState(
            AttackData data,
            string label)
        {
            _data = data;
            _label = label;
        }

        public bool IsFinished { get; private set; }
        private int TotalFrames => _data.StartupFrames + _data.ActiveFrames + _data.RecoveryFrames;
        
        public void Enter(FighterRuntime runtime)
        {
            _currentFrame = 0;
            _hasLunged = false;
            IsFinished = false;
            
            runtime.Services.HitDetector.BeginAttack();
            
            
            // Later:
            // runtime.Services.Animation.Play(_label);
        }

        public void Tick(FighterRuntime runtime)
        {
            TryLunge(runtime);
            
            runtime.Services.HitBoxManager.ApplyTimelineFrame(_data.boxTimeline, _currentFrame);
            runtime.Services.HitDetector.TryHit(runtime, _data);
            
            _currentFrame++;
            
            if (_currentFrame >= TotalFrames)
            {
                IsFinished = true;
            }
        }

        public void Exit(FighterRuntime runtime)
        {
            runtime.Services.HitBoxManager.ClearHitboxes();
            
            IsFinished = false;
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

            if (_currentFrame < _data.LungeFrame)
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