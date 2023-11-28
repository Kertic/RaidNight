
using Code.Entity.Player.StateMachines;
namespace Code.Entity.Player.StateMachines.BaseStates
{
    public abstract class State
    {
        
        public abstract void OnStateEnter();

        public abstract void OnStateExit();

        public abstract void StateUpdate();
    }
}