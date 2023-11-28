using Code.Entity.Player.StateMachines.BaseStates;

namespace Code.Entity.Player.StateMachines.FaeArcher.MischiefStates
{
    public abstract class MischiefState : State
    {
        protected MischiefStateMachine m_mischiefStateMachine;
        public abstract bool CanAddWisps();
        public MischiefState(MischiefStateMachine parentStateMachine)
        {
            m_mischiefStateMachine = parentStateMachine;
        }
    }
}