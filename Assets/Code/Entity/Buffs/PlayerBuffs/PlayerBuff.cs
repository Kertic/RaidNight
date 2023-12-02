using Code.Entity.Player;
using UnityEngine;

namespace Code.Entity.Buffs.PlayerBuffs
{
    public class PlayerBuff : Buff
    {
        protected PlayerEntity m_playerEntity;

        public PlayerBuff(PlayerEntity affectedEntity, float duration, Sprite buffIcon) : base(affectedEntity, duration, buffIcon)
        {
            m_playerEntity = affectedEntity;
        }
    }
}