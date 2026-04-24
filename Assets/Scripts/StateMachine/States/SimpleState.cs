using FighterBehaviour;
using FightTest.StateMachine;
using FightTest.Systems;

namespace FightTest.States
{
    public sealed class SimpleState : IState
    {
        private readonly ColliderSet _colliders;

        public SimpleState(ColliderSet colliders)
        {
            _colliders = colliders;
        }

        public void Enter(FighterRuntime runtime)
        {
            _colliders?.EnableSet();
        }

        public void Tick(FighterRuntime runtime)
        {
        }

        public void Exit(FighterRuntime runtime)
        {
            _colliders?.DisableSet();
        }
    }
}