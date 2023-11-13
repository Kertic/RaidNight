namespace Code.Player.States
{
    public abstract class IState
    {
        public abstract void OnStateEnter();

        public abstract void OnStateExit();

        public abstract void StateUpdate();
    }
}