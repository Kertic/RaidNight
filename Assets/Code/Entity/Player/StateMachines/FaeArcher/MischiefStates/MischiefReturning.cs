using Unity.Mathematics;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.FaeArcher.MischiefStates
{
    public class MischiefReturning : MischiefState
    {
        private float _returnCompleteTime;
        private Vector3 _startingPosition;

        public MischiefReturning(MischiefStateMachine parentStateMachine, float returnDuration) : base(parentStateMachine)
        {
            SetReturnCompleteTimeFromDuration(returnDuration);
        }

        public void SetReturnCompleteTimeFromDuration(float returnDuration)
        {
            _returnCompleteTime = returnDuration + Time.time;
        }

        public void SetReturnStartingPosition(Vector3 startingPosition)
        {
            _startingPosition = startingPosition;
        }

        public override void OnStateEnter()
        {
            m_mischiefStateMachine._MischiefView.SetMischiefPosition(_startingPosition);
            m_mischiefStateMachine._MischiefView.SetMischiefVisible(true);
        }

        public override void OnStateExit() { }

        public override void StateUpdate()
        {
            m_mischiefStateMachine._MischiefView.SetMischiefPosition(Vector3.Lerp(
                _startingPosition,
                m_mischiefStateMachine._PlayerFaeArcherStateMachine.transform.position,
                1.0f - math.tanh(Mathf.Max(0.0f, _returnCompleteTime - Time.time))
            ));

            if (Time.time >= _returnCompleteTime)
            {
                m_mischiefStateMachine.ChangeToIdle();
            }
        }

        public override bool CanAddWisps()
        {
            return false;
        }
    }
}