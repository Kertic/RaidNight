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

        public override void OnBuffEnter(BuffView[] buffViews)
        {
            base.OnBuffEnter(buffViews);
            _elapsedTicks = 0;
        }

        public override void OnBuffUpdate(BuffView[] buffViews)
        {
            base.OnBuffUpdate(buffViews);
            float percentComplete = GetTimeRemaining() / m_duration;
            float expectedTicks = (1.0f - percentComplete) * _totalTicks;
            
            while (expectedTicks >= _elapsedTicks)
            {
                m_entity.TakeDamage(_damagePerTick, new Color(0.8f, 0.5f, 0.8f, 1.0f));
                _elapsedTicks++;
            }
        }
    }
}