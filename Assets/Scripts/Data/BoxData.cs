using UnityEngine;

namespace Data
{
    [System.Serializable]
    public struct BoxData
    {
        public bool Enabled;
        public Vector2 Offset;
        public Vector2 Size;
    }
    
    [System.Serializable]
    public class BoxFrameData
    {
        public BoxData Pushbox;
        public BoxData[] Hurtboxes;
        public BoxData[] Hitboxes;
    }
}