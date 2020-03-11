using System;
using Game.view;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.entity.staticEntities
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
            var length = Math.Sqrt(Math.Pow(End.X - Start.X, 2) + Math.Pow(End.Y - Start.Y, 2));
            var angle  = Math.Atan2(End.Y - Start.Y, End.X - Start.X) - Math.PI / 2;

            spriteBatch.Draw(TextureStorage.Textures["Line"],
                             new Rectangle((int)Start.X, (int)Start.Y, (int)_width, (int)length), null, _color,
                             (float)angle, new Vector2(0.5f, 0), SpriteEffects.None, 0);
        }
    }
}
