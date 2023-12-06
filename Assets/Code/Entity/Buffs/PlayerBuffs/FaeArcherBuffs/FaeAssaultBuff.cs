using Code.Entity.Player;
using UnityEngine;

namespace Code.Entity.Buffs.PlayerBuffs.FaeArcherBuffs
{
    public class FaeAssaultBuff : PlayerBuff
    {
        private float _increaseAmount;
        private PlayerData.StatModifier _modifier;

        public FaeAssaultBuff(PlayerEntity affectedEntity, float duration, Sprite buffIcon, float increaseAmount) : base(affectedEntity, duration, buffIcon)
        {
            _increaseAmount = increaseAmount;
        }

        public override void OnBuffEnter(BuffView[] buffViews)
        {
            base.OnBuffEnter(buffViews);
            _modifier = new PlayerData.StatModifier(attSpeed =>
                {
                    attSpeed += _increaseAmount;
                    return attSpeed;
                },
                0
            );
            m_playerEntity._PlayerData.AddModifier(_modifier, PlayerData.StatType.ATTSPEED);
            m_playerEntity._PlayerData.AddModifier(_modifier, PlayerData.StatType.MOVESPEED);
        }

        public override void OnBuffExit(BuffView[] buffViews)
        {
            base.OnBuffExit(buffViews);
            m_playerEntity._PlayerData.RemoveModifier(_modifier, PlayerData.StatType.ATTSPEED);
            m_playerEntity._PlayerData.RemoveModifier(_modifier, PlayerData.StatType.MOVESPEED);
        }
    }
}