using FighterBehaviour;
using FightTest.Data;
using FightTest.StateMachine;
using FightTest.Systems;
using UnityEngine;

namespace FightTest.States
{
    public sealed class ThrowAttackState : IState
    {
        private readonly ColliderSet _colliders;
        private readonly AttackData _data;
        private readonly LayerMask _hitLayer;
        private readonly Collider2D[] _overlapBuffer = new Collider2D[8];
        private readonly GameObject _self;
        private bool _hasHitThisSwing;

        private float _timer;
        private bool _wasInActive;

        public ThrowAttackState(
            AttackData data,
            ColliderSet colliders,
            LayerMask hitLayer,
            GameObject self)
        {
            _data = data;
            _colliders = colliders;
            _hitLayer = hitLayer;
            _self = self;
        }

        public bool IsFinished { get; private set; }

        private float StartupDuration => _data.StartupFrames / 60f;
        private float ActiveDuration => _data.ActiveFrames / 60f;
        private float RecoveryDuration => _data.RecoveryFrames / 60f;
        private float TotalDuration => StartupDuration + ActiveDuration + RecoveryDuration;

        public void Enter(FighterRuntime runtime)
        {
            IsFinished = false;
            _hasHitThisSwing = false;
            _wasInActive = false;
            _timer = 0f;
            _colliders.EnableSet();
        }

        public void Tick(FighterRuntime runtime)
        {
            _timer += Time.deltaTime;

            var inActive = _timer >= StartupDuration && _timer < StartupDuration + ActiveDuration;

            if (inActive && !_wasInActive)
            {
                _colliders.EnableHitboxes();
                _wasInActive = true;
            }
            else if (!inActive && _wasInActive)
            {
                _colliders.DisableHitboxes();
            }

            if (inActive && !_hasHitThisSwing)
            {
                TryThrow();
            }

            if (_timer >= TotalDuration)
            {
                IsFinished = true;
            }
        }

        public void Exit(FighterRuntime runtime)
        {
            _colliders.DisableSet();
            IsFinished = false;
        }

        private void TryThrow()
        {
            var filter = new ContactFilter2D();
            filter.SetLayerMask(_hitLayer);
            filter.useTriggers = true;

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
                    hittable.ReceiveThrow(_data);
                    return;
                }
            }
        }
    }
}