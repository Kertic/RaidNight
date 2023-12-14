using Code.Entity.Buffs;
using Code.Entity.Buffs.PlayerBuffs.FaeArcherBuffs;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.FaeArcher.PlayerFaeArcherStates
{
    public class FaeAssault : ExecuteFaeSkill
    {
        private float _buffDuration, _attSpeedAmp;
        private Buff _faeAssaultBuff;
        private Sprite _buffIcon;

        public FaeAssault(PlayerData data, EntityPhysics entityPhysics, PlayerFaeArcherStateMachine controlsStateMachine, float buffDuration, float cooldown, float attackSpeedAmp, Sprite buffIcon) : base(data, entityPhysics, controlsStateMachine, cooldown)
        {
            _buffDuration = buffDuration;
            _attSpeedAmp = attackSpeedAmp;
            _buffIcon = buffIcon;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _faeAssaultBuff = new FaeAssaultBuff(m_controlsStateMachine, m_controlsStateMachine._PlayerEntity, _buffDuration, _buffIcon, _attSpeedAmp, 10.0f);
            m_controlsStateMachine._PlayerEntity.AddBuff(_faeAssaultBuff);
            m_controlsStateMachine.ChangeToIdleState();
        }

        public void SetBuffDuration(float newDuration)
        {
            _buffDuration = newDuration;
        }
      
    }
}