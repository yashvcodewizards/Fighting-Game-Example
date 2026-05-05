using Data;
using FightTest.Data;

namespace FightTest.Systems
{
    public interface IHittable
    {
        void ReceiveHit(HitInfo data);
        //void ReceiveThrow(AttackData data);
    }
}
