using UnityEngine;

namespace Data
{
    [System.Serializable]
    public struct ColliderData
    {
        public bool Enabled;
        public Vector2 Offset;
        public Vector2 Size;
    }
    
    [System.Serializable]
    public class ColliderFrameData
    {
        public ColliderData Pushbox;
        public ColliderData[] Hurtboxes;
        public ColliderData[] Hitboxes;
    }
}