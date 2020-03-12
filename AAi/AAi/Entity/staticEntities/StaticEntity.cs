using AAI.world;
using Microsoft.Xna.Framework;

namespace AAI.Entity.staticEntities
{
	public abstract class StaticEntity : BaseGameEntity
    {
        //public virtual bool IsInside(Vector2 position, float seperation = 0)
        //{
        //    return position.X >= Position.X          - seperation &&
        //           position.Y >= Position.Y          - seperation &&
        //           position.X <= Position.X + Size.X + seperation &&
        //           position.Y <= Position.Y + Size.Y + seperation;
        //}


        protected StaticEntity(Vector2 pos, World w) : base(pos, w)
        {
        }
    }
}