using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.view;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.entity.staticEntities
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
                             new Rectangle((int)Pos.X, (int)Pos.Y, (int) Scale, (int) Scale),
                             null,
                             Color.White);
        }
    }
}
