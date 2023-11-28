using System.Net.NetworkInformation;
using Code.Entity.Player.StateMachines.BaseStates;
using Code.Systems;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.FaeArcher.MischiefStates
{
    public class MischiefIdle : MischiefState
    {

        private float _mischiefSpinSpeed, _mischiefRadius;
        private Transform _trackedTransform;

        public MischiefIdle(MischiefStateMachine parentStateMachine, float spinSpeed, float spinRadius, Transform defaultTrackedTransform) : base(parentStateMachine)
        {
            m_mischiefStateMachine = parentStateMachine;
            _mischiefSpinSpeed = spinSpeed;
            _mischiefRadius = spinRadius;
            SetTrackedTransform(defaultTrackedTransform);
        }

        private void OnHitEntity(Entity hitEntity)
        {
            m_mischiefStateMachine.FireMischief(hitEntity);
        }

        public override void OnStateEnter()
        {
            m_mischiefStateMachine._MischiefView.SetMischiefVisible(true);
            m_mischiefStateMachine._PlayerFaeArcherStateMachine.m_onHitEntity += OnHitEntity;
        }


        public override void OnStateExit()
        {
            m_mischiefStateMachine._MischiefView.SetMischiefVisible(false);
            m_mischiefStateMachine._PlayerFaeArcherStateMachine.m_onHitEntity -= OnHitEntity;
        }

        public void SetTrackedTransform(Transform newTrackedTransform)
        {
            _trackedTransform = newTrackedTransform;
        }

        public override void StateUpdate()
        {
            float rotationOffset = Time.time * _mischiefSpinSpeed;
            Vector3 newMischiefPos = Utils.Vector3.GetPositionInRotatingCircle(1, 1, rotationOffset, _mischiefRadius, _trackedTransform.position);
            m_mischiefStateMachine._MischiefView.SetMischiefPosition(newMischiefPos);
            m_mischiefStateMachine._MischiefView.SetMischiefRotation(Quaternion.Euler(0, 0,
                Utils.Vector2.GetRotationOfObjectOnCircle(newMischiefPos, _trackedTransform.position)));
        }

        public override bool CanAddWisps()
        {
            return true;
        }
    }
}