using System.Collections.Generic;
using Data;
using UnityEngine;

namespace FightTest.Systems
{
    public class HitBoxManager : MonoBehaviour
    {
        [Header("Box Slots")] [SerializeField] private BoxCollider2D[] hitboxSlots;
        [SerializeField] private BoxCollider2D[] hurtboxSlots;
        [SerializeField] private CapsuleCollider2D pushboxSlot;
        
        public IReadOnlyList<BoxCollider2D> ActiveHitBoxes => hitboxSlots;

        public void ApplyTimelineFrame(ColliderTimeline timeline, int frame)
        {
            if (timeline == null)
            {
                ClearHitboxes();
                return;
            }

            var boxFrame = timeline.GetFrame(frame);

            if (boxFrame == null)
            {
                ClearHitboxes();
                return;
            }

            ApplyFrame(boxFrame);
        }

        public void ApplyProfile(ColliderProfile profile)
        {
            if (profile == null || profile.Frame == null)
            {
                Debug.LogError("HitBoxManager.ApplyProfile:: Profile or Frame is null", this);
                ClearAll();
                return;
            }

            ApplyFrame(profile.Frame);
        }

        public void ApplyFrame(ColliderFrameData frame)
        {
            if (frame == null)
            {
                ClearAll();
                return;
            }

            ApplyPushbox(frame.Pushbox);
            ApplyHurtboxes(frame.Hurtboxes);
            ApplyHitboxes(frame.Hitboxes);
        }

        public void ApplyHitboxes(ColliderShapeData[] boxes)
        {
            ApplyBoxesToSlots(hitboxSlots, boxes);
        }

        public void ApplyHurtboxes(ColliderShapeData[] boxes)
        {
            ApplyBoxesToSlots(hurtboxSlots, boxes);
        }

        public void ApplyPushbox(ColliderShapeData colliderShape)
        {
            if (pushboxSlot == null)
            {
                Debug.LogError("HitBoxManager.ApplyPushbox:: Pushbox is null");
                return;
            }

            ApplyBox(pushboxSlot, colliderShape);
        }

        public void ClearHitboxes()
        {
            ClearSlots(hitboxSlots);
        }

        public void ClearAll()
        {
            ClearSlots(hitboxSlots);
            ClearSlots(hurtboxSlots);

            if (pushboxSlot != null)
            {
                pushboxSlot.enabled = false;
            }
        }

        private void ApplyBoxesToSlots(Collider2D[] slots, ColliderShapeData[] boxes)
        {
            if (slots == null)
            {
                return;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (boxes != null && i < boxes.Length)
                {
                    ApplyBox(slots[i], boxes[i]);
                }
                else if (slots[i] != null)
                {
                    slots[i].enabled = false;
                }
            }
        }

        private void ApplyBox(Collider2D col, ColliderShapeData shapeData)
        {
            if (col == null)
            {
                return;
            }

            col.enabled = shapeData.Enabled;

            if (!shapeData.Enabled)
            {
                return;
            }

            col.offset = shapeData.Offset;

            if (col is BoxCollider2D box)
            {
                box.size = shapeData.Size;
            }
            else if (col is CapsuleCollider2D capsule)
            {
                capsule.size = shapeData.Size;
            }
            else
            {
                Debug.LogWarning($"Unsupported collider type: {col.GetType().Name}", col);
            }
        }

        private void ClearSlots(Collider2D[] slots)
        {
            if (slots == null)
            {
                return;
            }

            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    slot.enabled = false;
                }
            }
        }
    }
}