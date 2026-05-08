using Data;
using UnityEngine;

namespace FightTest.Systems
{
    [CreateAssetMenu(menuName = "Fight Test/Combat Boxes/Box Timeline")]
    public class BoxTimeline : ScriptableObject
    {
        public BoxTimelineData[] Entries;

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