using AAI.world;
using Microsoft.Xna.Framework;

namespace AAI.Entity.staticEntities
{
	public abstract class StaticEntity : BaseGameEntity
    {
        public virtual bool IsInside(Vector2 position, float seperation = 0)
        {
            return position.X >= Pos.X - seperation &&
                   position.Y >= Pos.Y - seperation &&
                   position.X <= Pos.X + Size.X + seperation &&
                   position.Y <= Pos.Y + Size.Y + seperation;
        }


        protected StaticEntity(Vector2 pos, World w) : base(pos, w)
        {
        }
    }
}