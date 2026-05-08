namespace FightTest.States
{
    public sealed class StateFrameTimer
    {
        public int CurrentFrame { get; private set; }
        public int DurationFrames { get; private set; }

        public bool IsFinished => CurrentFrame >= DurationFrames;

        public void Start(int durationFrames = 0)
        {
            CurrentFrame = 0;
            DurationFrames = durationFrames;
        }

        public void Tick()
        {
            CurrentFrame++;
        }

        public void Reset()
        {
            CurrentFrame = 0;
            DurationFrames = 0;
        }

        public void Restore(int currentFrame, int durationFrames)
        {
            CurrentFrame = currentFrame;
            DurationFrames = durationFrames;
        }
    }
}