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

        private float _timer;
        private bool _hasLunged;

        //private bool _hasHitThisSwing;
        private bool _activeStarted;
        private bool _activeEnded;

        public AttackState(
            AttackData data,
            string label)
        {
            _data = data;
            _label = label;
        }

        public bool IsFinished { get; private set; }

        private float StartupDuration => _data.StartupFrames / 60f;
        private float ActiveDuration => _data.ActiveFrames / 60f;
        private float RecoveryDuration => _data.RecoveryFrames / 60f;
        private float TotalDuration => StartupDuration + ActiveDuration + RecoveryDuration;


        public void Enter(FighterRuntime runtime)
        {
            _timer = 0f;
            _hasLunged = false;
            _activeStarted = false;
            _activeEnded = false;
            IsFinished = false;
            
            
            // Later:
            // runtime.Services.Animation.Play(_label);
            // runtime.Services.HitboxManager.BeginAttack(_data);
            //_hasHitThisSwing = false; -> move to HixBox Hithanddler

            //_colliders?.EnableSet();
        }

        public void Tick(FighterRuntime runtime)
        {
            _timer += Time.deltaTime;

            TryLunge(runtime);
            UpdateActiveWindow(runtime);

            if (_timer >= TotalDuration)
            {
                IsFinished = true;
            }
        }

        public void Exit(FighterRuntime runtime)
        {
            //_colliders?.DisableSet();
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

            if (_timer < _data.LungeFrame / 60f)
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

        private void UpdateActiveWindow(FighterRuntime runtime)
        {
            var activeStart = StartupDuration;
            var activeEnd = StartupDuration + ActiveDuration;

            if (!_activeStarted && _timer >= activeStart)
            {
                _activeStarted = true;

                // Later:
                // runtime.Services.HitboxManager.EnableHitboxes(_data);
            }

            if (!_activeEnded && _timer >= activeEnd)
            {
                _activeEnded = true;

                // Later:
                // runtime.Services.HitboxManager.DisableHitboxes();
            }
        }


        /*private void TryHit()
        {
            var filter = new ContactFilter2D();
            filter.SetLayerMask(_hitLayer);
            filter.useTriggers = true;

            if (!_colliders)
            {
                return;
            }

            foreach (var hitbox in _colliders.Hitboxes)
            {
                var count = hitbox.OverlapCollider(filter, _overlapBuffer);
                for (var i = 0; i < count; i++)
                {
                    if (_overlapBuffer[i].transform.IsChildOf(_self.transform))
                    {
                        continue;
                    }

                    var hittable = _overlapBuffer[i].GetComponentInParent<IHittable>();
                    if (hittable == null)
                    {
                        continue;
                    }

                    _hasHitThisSwing = true;
                    hittable.ReceiveHit(_data);
                    return;
                }
            }
        }*/
    }
}