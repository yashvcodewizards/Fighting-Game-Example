using Data;

namespace FightTest.Systems
{
    [System.Serializable]
    public class ColliderTimelineData
    {
        public int StartFrame;
        public int EndFrame;
        public ColliderFrameData Frame;

        public bool Contains(int frame)
        {
            return frame >= StartFrame && frame <= EndFrame;
        }
    }
}