using Data;
using FightTest.StateMachine;

namespace FightTest.Systems
{
    public interface IHittable
    {
        HitReactionType ReceiveHit(HitInfo data);
        //void ReceiveThrow(AttackData data);
    }
}
