using Data;
using FighterBehaviour;
using FighterBehaviour.FighterBehaviours;
using FightTest.Data;
using FightTest.Systems;
using UnityEngine;

namespace FightTest.States.FoitingueStates
{
    [CreateAssetMenu(menuName = "FightTest/Special States/Test Throw")]
    public sealed class TestThrowSpecialState : SpecialStateAsset
    {
        [Header("Timing")]
        [SerializeField] private int startupFrames = 10;
        [SerializeField] private int executionFrames = 90;
        [SerializeField] private int whiffRecoveryFrames = 25;

        [Header("Targeting")]
        [SerializeField] private float grabRange = 1.5f;
        [SerializeField] private LayerMask hurtboxLayer;

        [Header("Placement")]
        [SerializeField] private float targetOffset = 1.25f;

        [Header("Result")]
        [SerializeField] private AttackData throwResultData;

        [Header("Presentation")]
        [SerializeField] private string throwAnimation = "Throw";

        private FighterRuntime _target;
        private bool _hasTriedGrab;
        private bool _hasCaptured;
        private bool _hasReleasedTarget;

        private int ReleaseFrame => startupFrames + executionFrames - 1;

        public override void Enter(FighterRuntime runtime)
        {
            _target = null;
            _hasTriedGrab = false;
            _hasCaptured = false;
            _hasReleasedTarget = false;

            runtime.Services.StateTimer.SetDuration(startupFrames + executionFrames);
            runtime.Services.Presentation.Play(throwAnimation);
        }

        public override void Tick(FighterRuntime runtime)
        {
            var frame = runtime.Services.StateTimer.CurrentFrame;

            // Startup runs from frame 0 to startupFrames - 1.
            // Grab is attempted once on the final startup frame.
            if (!_hasTriedGrab && frame == startupFrames - 1)
            {
                _hasTriedGrab = true;

                _target = TryFindTarget(runtime);

                if (_target == null)
                {
                    StartWhiffRecovery(runtime, frame);
                    return;
                }

                CaptureTarget(runtime, _target);
            }

            // Release happens once on the final execution frame.
            if (_hasCaptured && !_hasReleasedTarget && frame == ReleaseFrame)
            {
                ReleaseTargetIntoHitStun(runtime);
                _hasReleasedTarget = true;
            }
        }

        public override void Exit(FighterRuntime runtime)
        {
            _target = null;
        }

        private void StartWhiffRecovery(FighterRuntime runtime, int currentFrame)
        {
            runtime.Services.StateTimer.SetDuration(currentFrame + 1 + whiffRecoveryFrames);
        }

        private FighterRuntime TryFindTarget(FighterRuntime attacker)
        {
            var origin = attacker.Services.Self.transform.position;
            var direction = Vector2.right * attacker.Services.Facing.Sign;

            var hits = Physics2D.RaycastAll(
                origin,
                direction,
                grabRange,
                hurtboxLayer
            );

            foreach (var hit in hits)
            {
                if (!hit.collider)
                {
                    continue;
                }

                if (hit.collider.transform.IsChildOf(attacker.Services.Self.transform))
                {
                    continue;
                }

                var hitHandler = hit.collider.GetComponentInParent<HitHandler>();

                if (hitHandler == null)
                {
                    continue;
                }

                return hitHandler.Runtime;
            }

            return null;
        }

        private void CaptureTarget(FighterRuntime attacker, FighterRuntime target)
        {
            _hasCaptured = true;

            var captured = target.Services.Root.StateRegistry.Get(BasicFighterStateKeys.Captured);

            target.Services.Root.ChangeState(captured);

            // One-frame placement event.
            DriveTarget(attacker, target);
        }

        private void DriveTarget(FighterRuntime attacker, FighterRuntime target)
        {
            var attackerTransform = attacker.Services.Self.transform;
            var targetTransform = target.Services.Self.transform;

            var offset = new Vector3(
                -attacker.Services.Facing.Sign * targetOffset,
                0f,
                0f
            );

            targetTransform.position = attackerTransform.position + offset;
        }

        private void ReleaseTargetIntoHitStun(FighterRuntime attacker)
        {
            if (_target == null || throwResultData == null)
            {
                return;
            }

            _target.Context.PendingHit = new HitInfo(
                throwResultData,
                new Vector2(attacker.Services.Facing.Sign, 0f)
            );

            var hitStun = _target.Services.Root.StateRegistry.Get(BasicFighterStateKeys.HitStun);

            _target.Services.Root.ChangeState(hitStun);
        }
    }
}