using Data;

namespace FightTest.Systems
{
    [System.Serializable]
    public class BoxTimelineData
    {
        public int StartFrame;
        public int EndFrame;
        public BoxFrameData Frame;

        public bool Contains(int frame)
        {
            return frame >= StartFrame && frame <= EndFrame;
        }
    }
}