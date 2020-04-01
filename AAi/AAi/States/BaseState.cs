namespace AAI.States
{
    public abstract class BaseState<T>
    {
        public abstract void Enter(T   t);
        public abstract void Execute(T t);
        public abstract void Exit(T    t);
    }
}