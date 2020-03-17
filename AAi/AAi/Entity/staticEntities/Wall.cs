using System;
using AAI.View;
using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Entity.staticEntities
{
    class Wall : StaticEntity
    {
        public Vector2 Start;
        public Vector2 End;

        private int   _width;
        private Color _color;

        public Wall(Vector2 pos, World w, Vector2 end, int width, Color color) : base(pos, w)
        {
            Start   = pos;
            End     = end;
            _width = width;
            _color = color;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Vector2 edge = End - Start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            //spriteBatch.Draw(TextureStorage.Textures["Line"],
            //                 new Rectangle((int)Start.X, (int)Start.Y, (int)_width, (int)length), null, _color,
            //                 (float)angle, new Vector2(0.5f, 0), SpriteEffects.None, 0);
            /////////////////////////////////////////
            Texture = TextureStorage.Textures["Line"];
            var origin = new Vector2(0);
            spriteBatch.Draw(Texture,
                             new Rectangle( // rectangle defines shape of line and position of start of line
                                           (int)Start.X,
                                           (int)Start.Y,
                                           (int)edge.Length(), //sb will strech the texture to fill this rectangle
                                           _width),                 //width of line, change this to make thicker line
                             null,
                             _color,         //colour of line
                             angle,             //angle of line (calulated above)
                             new Vector2(0, 0.5f), // point in line about which to rotate
                             SpriteEffects.None,
                             0);
        }
    }
}
