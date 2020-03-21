using AAI.View;
using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Entity.staticEntities
{
    public class Target : StaticEntity
    {
        public Target(Vector2 pos, World w) : base(pos, w)
        {
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Texture = TextureStorage.Textures["Target"];
            spriteBatch.Draw(Texture,
                             new Rectangle((int) (Pos.X - (Scale / 2)), (int)(Pos.Y - (Scale / 2)), (int) Scale, (int) Scale),
                             null,
                             Color.White);
        }
    }
}
