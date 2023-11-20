namespace Code.Entity.Player.StateMachines
{
    public abstract class State
    {
        public abstract void OnStateEnter();

        public abstract void OnStateExit();

        public abstract void StateUpdate();
    }
}