using Code.Player.Weapon;

namespace Code.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates
{
    public class FireEnchantedArrow : SuperStates.ExecuteSkill
    {
        public FireEnchantedArrow(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine) : base(data, entityPhysics, controlsStateMachine) { }
    }
}