using UnityEngine;

namespace FightTest.Systems
{
    public class FighterPresentation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void Play(string animationName)
        {
            if (_animator == null || string.IsNullOrWhiteSpace(animationName))
            {
                return;
            }

            _animator.Play(animationName);
        }
    }
}