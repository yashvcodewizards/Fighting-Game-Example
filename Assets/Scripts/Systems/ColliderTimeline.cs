using Data;
using UnityEngine;

namespace FightTest.Systems
{
    [CreateAssetMenu(menuName = "Fight Test/Combat Boxes/Collider Timeline")]
    public class ColliderTimeline : ScriptableObject
    {
        public ColliderTimelineData[] Entries;

        public ColliderFrameData GetFrame(int frame)
        {
            if (Entries == null)
            {
                return null;
            }

            foreach (var entry in Entries)
            {
                if (entry.Contains(frame))
                {
                    return entry.Frame;
                }
            }

            return null;
        }
        
        public int TotalFrames
        {
            get
            {
                if (Entries == null || Entries.Length == 0)
                {
                    return 0;
                }

                var maxFrame = 0;

                foreach (var entry in Entries)
                {
                    if (entry.EndFrame > maxFrame)
                    {
                        maxFrame = entry.EndFrame;
                    }
                }

                return maxFrame + 1;
            }
        }
    }
}