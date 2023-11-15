using Code.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates;
using UnityEngine;

namespace Code.Player.StateMachines
{
    [RequireComponent(typeof(PlayerData), typeof(EntityPhysics))]
    public class PlayerFaeArcherStateMachine : PlayerControlsStateMachine
    {

        [SerializeField]
        private Weapon.Weapon enchantedArrowWeapon;
        private Flit _Flit { get; set; }
        private FireEnchantedArrow _FireEnchantedArrow { get; set; }

        protected override void Awake()
        {
            base.Awake();
            _Flit = new Flit(PlayerData, EntityPhysics, this);
            _FireEnchantedArrow = new FireEnchantedArrow(PlayerData, EntityPhysics, this);
        }
        
        
        
    }
}