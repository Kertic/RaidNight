using Code.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates;
using Code.Player.StateMachines.PlayerWeaponStates;
using UnityEngine;

namespace Code.Player.StateMachines
{
    [RequireComponent(typeof(PlayerData), typeof(EntityPhysics))]
    public class PlayerFaeArcherStateMachine : PlayerControlsStateMachine
    {
        [SerializeField] private Weapon.Weapon enchantedArrowWeapon;
        [SerializeField] private float enchantedArrowProjectileSpeed;
        [SerializeField] private float fireEnchantedArrowCastTime;
        [SerializeField] private float flitMaxDistance;
        [SerializeField] private float flitDuration;

        private Flit _Flit { get; set; }
        private FireEnchantedArrow _FireEnchantedArrow { get; set; }

        protected override void Awake()
        {
            base.Awake();
            _Dash = _Flit = new Flit(PlayerData, EntityPhysics, this, flitMaxDistance,flitDuration );
            _PrimaryAttack = _FireEnchantedArrow = new FireEnchantedArrow(PlayerData, EntityPhysics, this, castBarView);
        }

        public override void ChangeToPrimaryAttack()
        {
            _FireEnchantedArrow.SetCastTime(fireEnchantedArrowCastTime);
            base.ChangeToPrimaryAttack();
        }

        public override void ChangeToDash()
        {
            _Flit.SetDashDuration(flitDuration);
            _Flit.SetDashDistance(flitMaxDistance);
            base.ChangeToDash();
        }

        public void FireEnchantedArrowWeapon(Vector2 targetLocation)
        {
            enchantedArrowWeapon.FireProjectile(targetLocation - (Vector2)transform.position, enchantedArrowProjectileSpeed);
        }
    }
}