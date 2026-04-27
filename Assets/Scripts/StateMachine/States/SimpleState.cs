using FighterBehaviour;
using FightTest.StateMachine;
using FightTest.Systems;

namespace FightTest.States
{
    /// <summary>
    /// Basic state with no special behaviour.
    /// Used for states that only need to exist as part of the state graph.
    /// </summary>
    public sealed class SimpleState : IState
    {
        public void Enter(FighterRuntime runtime)
        {
        }

        public void Tick(FighterRuntime runtime)
        {
        }

        public void Exit(FighterRuntime runtime)
        {
        }
    }
}