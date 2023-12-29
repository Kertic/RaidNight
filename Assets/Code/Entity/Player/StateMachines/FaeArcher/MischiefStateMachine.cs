using System;
using Code.Entity.Player.StateMachines.FaeArcher.MischiefStates;
using Code.Entity.Player.Views.FaeArcher;
using Code.Entity.Player.Weapon;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.FaeArcher
{
    [RequireComponent(typeof(MischiefView))]
    public class MischiefStateMachine : StateMachine<MischiefState>
    {
        [SerializeField]
        private TrackingWeapon mischiefWeapon;

        [SerializeField]
        private MischiefView mischiefView;

        [SerializeField]
        private PlayerFaeArcherStateMachine faeArcherStateMachine;

        [SerializeField]
        private float mischiefSpinSpeed, mischiefSpinRadius, returnDuration;

        [Header("Mischief Projectile Info")]
        [SerializeField]
        private float mischiefProjectileSpeed;

        protected MischiefIdle _MischiefIdle { get; set; }
        protected MischiefAttacking _MischiefAttacking { get; set; }
        protected MischiefReturning _MischiefReturning { get; set; }

        public MischiefView _MischiefView => mischiefView;
        public PlayerFaeArcherStateMachine _PlayerFaeArcherStateMachine => faeArcherStateMachine;
        public Action<TrackingProjectile> m_onFiredTrackingProjectile;

        private void Awake()
        {
            _MischiefIdle = new MischiefIdle(this, mischiefSpinSpeed, mischiefSpinRadius, faeArcherStateMachine.transform);
            _MischiefAttacking = new MischiefAttacking(this);
            _MischiefReturning = new MischiefReturning(this, returnDuration);
        }

        protected override void Start()
        {
            base.Start();
            ChangeToIdle();
        }

        public void ChangeToReturning(Vector3 startingPos)
        {
            _MischiefReturning.SetReturnCompleteTimeFromDuration(returnDuration);
            _MischiefReturning.SetReturnStartingPosition(startingPos);
            ChangeState(_MischiefReturning);
        }

        public void ChangeToIdle()
        {
            ChangeState(_MischiefIdle);
        }

        public void FireMischief(Entity targetEntity)
        {
            ChangeState(_MischiefAttacking);
            TrackingProjectile projectile = mischiefWeapon.FireProjectile(targetEntity, mischiefProjectileSpeed);
            projectile.m_onEntityHit += hit2Ds => { ChangeToReturning(hit2Ds[0].point); };
            m_onFiredTrackingProjectile?.Invoke(projectile);
        }

        public bool GetCanAddWisps()
        {
            return m_currentState.CanAddWisps();
        }
    }
}