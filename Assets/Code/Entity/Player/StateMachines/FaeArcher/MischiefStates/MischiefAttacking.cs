namespace Code.Entity.Player.StateMachines.FaeArcher.MischiefStates
{
    public class MischiefAttacking : MischiefState
    {
        public MischiefAttacking(MischiefStateMachine parentStateMachine) : base(parentStateMachine) { }
        public override void OnStateEnter() { }

        public override void OnStateExit() { }

        public override void StateUpdate() { }
        public override bool CanAddWisps()
        {
            return false;
        }
    }
}