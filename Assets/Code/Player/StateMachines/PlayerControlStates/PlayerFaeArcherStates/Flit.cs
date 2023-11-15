using Code.Player.StateMachines.PlayerControlStates.SubStates.ExecuteSkill;

namespace Code.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates
{
    public class Flit : Dash
    {
        public Flit(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine) : base(data, entityPhysics, controlsStateMachine) { }
    }
}