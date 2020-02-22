using entity;
using Microsoft.Xna.Framework;

internal abstract class SteeringBehaviour
{
    public MovingEntity ME { get; set; }

    public SteeringBehaviour(MovingEntity me)
    {
        ME = me;
    }

    public abstract Vector2 Calculate();
}