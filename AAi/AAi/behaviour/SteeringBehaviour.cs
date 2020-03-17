using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public abstract void DebugDraw(SpriteBatch spriteBatch, float scale);
    }
}