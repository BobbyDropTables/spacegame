using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace AAI.behaviour
{
    public abstract class SteeringBehaviour
    {
        public MovingEntity ME { get; set; }

        public SteeringBehaviour(MovingEntity me)
        {
            ME = me;
        }

        public abstract Vector2 Calculate();
    }
}