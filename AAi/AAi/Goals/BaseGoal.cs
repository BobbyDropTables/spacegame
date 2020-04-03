using AAI.Entity.MovingEntities;

namespace AAI.Goals
{

    public enum Statusgoal
    {
        inactive,
        active,
        completed,
    }

    public abstract class BaseGoal
    {
        protected Vehicle Vehicle;
        public Statusgoal State;
        public string Name;
        public abstract void Activate();

        public virtual Statusgoal Process()
        {
            return Statusgoal.inactive;
        }

        public virtual void AddSubGoal(BaseGoal goal) { }
    }
}