using FighterBehaviour;

namespace FightTest.StateMachine
{
    public interface IState
    {
        void Enter(FighterRuntime runtime);
        void Tick(FighterRuntime runtime);
        void Exit(FighterRuntime runtime);
    }
}