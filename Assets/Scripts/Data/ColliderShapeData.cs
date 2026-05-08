using UnityEngine;

namespace Data
{
    [System.Serializable]
    public struct ColliderShapeData
    {
        public bool Enabled;
        public Vector2 Offset;
        public Vector2 Size;
    }
    
    [System.Serializable]
    public class ColliderFrameData
    {
        public ColliderShapeData Pushbox;
        public ColliderShapeData[] Hurtboxes;
        public ColliderShapeData[] Hitboxes;
    }
}