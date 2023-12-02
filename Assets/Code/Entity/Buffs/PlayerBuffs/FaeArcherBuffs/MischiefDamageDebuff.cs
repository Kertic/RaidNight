using Code.Entity.Player;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Entity.Buffs.PlayerBuffs.FaeArcherBuffs
{
    public class MischiefDamageDebuff : Buff
    {
        private float _damagePerTick;
        private int _totalTicks;

        private int _elapsedTicks;

        public MischiefDamageDebuff(Entity affectedEntity, float duration, Sprite buffIcon, float totalDamage, int totalDamageTicks) : base(affectedEntity, duration, buffIcon)
        {
            _damagePerTick = totalDamage / totalDamageTicks;
            _totalTicks = totalDamageTicks;
        }

        public override void OnBuffEnter(BuffView buffview)
        {
            base.OnBuffEnter(buffview);
            _elapsedTicks = 0;
        }

        public override void OnBuffUpdate(BuffView buffview)
        {
            base.OnBuffUpdate(buffview);
            float percentComplete = GetTimeRemaining() / m_duration;
            float expectedTicks = (1.0f - percentComplete) * _totalTicks;
            
            while (expectedTicks >= _elapsedTicks)
            {
                m_entity.TakeDamage(_damagePerTick);
                _elapsedTicks++;
            }
        }
    }
}