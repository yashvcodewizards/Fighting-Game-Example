using FightTest.Data;
using UnityEngine;

namespace FightTest.Input
{
    public class AiInputProvider : MonoBehaviour, IInputProvider
    {
        public InputFrame GetFrame()
        {
            return new InputFrame(0f, 0f, false, false);
        }
    }
}
