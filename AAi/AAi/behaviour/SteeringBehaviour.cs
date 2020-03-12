using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace AAI.behaviour
{
    abstract class SteeringBehaviour
    {
        public MovingEntity ME { get; set; }
        public abstract Vector2 Calculate();

        public SteeringBehaviour(MovingEntity me)
        {
            ME = me;
        }
    }

    
}
