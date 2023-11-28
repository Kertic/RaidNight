using Code.Entity.Player.StateMachines.BaseStates.PlayerControlStates.SuperStates;

namespace Code.Entity.Player.StateMachines.FaeArcher.PlayerFaeArcherStates
{
    public class ExecuteFaeSkill : ExecuteSkill
    {
        protected new PlayerFaeArcherStateMachine m_controlsStateMachine;

        public ExecuteFaeSkill(PlayerData data, EntityPhysics entityPhysics, PlayerFaeArcherStateMachine controlsStateMachine, float cooldown) : base(data, entityPhysics, controlsStateMachine, cooldown)
        {
            m_controlsStateMachine = controlsStateMachine;
        }
    }
}