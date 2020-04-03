using AAI.View;
using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Entity.staticEntities
{
    public class Target : BaseGameEntity
    {
        public Target(Vector2 pos, World w) : base(pos, w)
        {
        }

        public void Render(SpriteBatch spriteBatch, Texture2D texture)
        {
            Texture = texture;
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(
                             Texture,
                             new Rectangle(
                                           Pos.ToPoint(),
                                           Size.ToPoint()),
                             null,
                             Color.White,
                             0f,
                             origin,
                             SpriteEffects.None,
                             0f);
        }
    }
}
