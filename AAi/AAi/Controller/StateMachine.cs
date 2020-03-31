using AAI.States;

namespace AAI.Controller
{
    public class StateMachine<T>
    {
        public T            Entity;
        public BaseState<T> State;

        public StateMachine(T entity)
        {
            this.Entity = entity;
        }
        public void Changestate(BaseState<T> state)
        {
            State.Exit(Entity);
            State = state;
            State.Enter(Entity);
        }

        public void Update()
        {
            State.Execute(Entity);
        }
    }
}