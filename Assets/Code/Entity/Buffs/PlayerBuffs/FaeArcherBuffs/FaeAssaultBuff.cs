using Code.Entity.Player;
using Code.Entity.Player.StateMachines.FaeArcher;
using Code.Entity.Player.Weapon.OnHitEffects;
using UnityEngine;

namespace Code.Entity.Buffs.PlayerBuffs.FaeArcherBuffs
{
    public class FaeAssaultBuff : PlayerBuff
    {
        private float _increaseAmount;
        private PlayerData.StatModifier _modifier;
        private PlayerFaeArcherStateMachine _stateMachine;
        private DealDamageOnHit _onHit;

        public FaeAssaultBuff(PlayerFaeArcherStateMachine stateMachine, PlayerEntity affectedEntity, float duration, Sprite buffIcon, float increaseAmount, float onHitDamageAmount) : base(affectedEntity, duration, buffIcon)
        {
            _increaseAmount = increaseAmount;
            _stateMachine = stateMachine;
            _onHit = new DealDamageOnHit(onHitDamageAmount);
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
            _stateMachine._OnHitEffects.Add(_onHit);
        }

        public override void OnBuffExit(BuffView[] buffViews)
        {
            base.OnBuffExit(buffViews);
            m_playerEntity._PlayerData.RemoveModifier(_modifier, PlayerData.StatType.ATTSPEED);
            m_playerEntity._PlayerData.RemoveModifier(_modifier, PlayerData.StatType.MOVESPEED);
            _stateMachine._OnHitEffects.Remove(_onHit);
        }
    }
}