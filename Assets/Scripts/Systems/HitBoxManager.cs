using Data;
using UnityEngine;

namespace FightTest.Systems
{
    public class HitBoxManager: MonoBehaviour
    {
        [Header("Box Slots")]
        [SerializeField] private BoxCollider2D[] hitboxSlots;
        [SerializeField] private BoxCollider2D[] hurtboxSlots;
        [SerializeField] private CapsuleCollider2D pushboxSlot;

        public void ApplyProfile(BoxProfile profile)
        {
            if (profile == null || profile.Frame == null)
            {
                Debug.LogError("HitBoxManager.ApplyProfile:: Could not ApplyProfile, profile is null");
                ClearAll();
                return;
            }

            ApplyFrame(profile.Frame);
        }

        public void ApplyFrame(BoxFrameData frame)
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

        public void ApplyHitboxes(BoxData[] boxes)
        {
            ApplyBoxesToSlots(hitboxSlots, boxes);
        }

        public void ApplyHurtboxes(BoxData[] boxes)
        {
            ApplyBoxesToSlots(hurtboxSlots, boxes);
        }

        public void ApplyPushbox(BoxData box)
        {
            if (pushboxSlot == null)
            {
                Debug.LogError("HitBoxManager.ApplyPushbox:: Pushbox is null");
                return;
            }

            ApplyBox(pushboxSlot, box);
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
                pushboxSlot.enabled = false;
        }

        private void ApplyBoxesToSlots(Collider2D[] slots, BoxData[] boxes)
        {
            if (slots == null)
                return;

            for (int i = 0; i < slots.Length; i++)
            {
                if (boxes != null && i < boxes.Length)
                {
                    ApplyBox(slots[i], boxes[i]);
                }
                else
                {
                    slots[i].enabled = false;
                }
            }
        }

        private void ApplyBox(Collider2D col, BoxData data)
        {
            if (col == null)
            {
                return;
            }

            col.enabled = data.Enabled;

            if (!data.Enabled)
            {
                return;
            }

            col.offset = data.Offset;

            if (col is BoxCollider2D box)
            {
                box.size = data.Size;
            }
            else if (col is CapsuleCollider2D capsule)
            {
                capsule.size = data.Size;
            }
            else
            {
                Debug.LogWarning($"Unsupported collider type: {col.GetType().Name}", col);
            }
        }

        private void ClearSlots(Collider2D[] slots)
        {
            if (slots == null)
                return;

            foreach (var slot in slots)
            {
                if (slot != null)
                    slot.enabled = false;
            }
        }
    }
}