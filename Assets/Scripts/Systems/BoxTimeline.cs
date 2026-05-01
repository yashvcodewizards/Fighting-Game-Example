using Data;
using UnityEngine;

namespace FightTest.Systems
{
    [CreateAssetMenu(menuName = "Fight Test/Combat Boxes/Box Timeline")]
    public class BoxTimeline : ScriptableObject
    {
        public BoxTimelineData[] Entries;

        public BoxFrameData GetFrame(int frame)
        {
            if (Entries == null)
                return null;

            foreach (var entry in Entries)
            {
                if (entry.Contains(frame))
                    return entry.Frame;
            }

            return null;
        }
    }
}