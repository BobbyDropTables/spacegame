using AAI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace entity
{
    internal class Vehicle : MovingEntity
    {
        public bool      target { get; set; }
        public Texture2D Texture;

        public Vehicle(Vector2 pos, World.World w, Texture2D texture) : base(pos, w, texture)
        {
            Velocity = new Vector2(0, 0);
            Scale    = 20;
            this.Texture = texture;
            target = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            double leftCorner  = Pos.X - Scale;
            double rightCorner = Pos.Y - Scale;
            int size        = (int)Scale * 2;
            spriteBatch.Draw(Texture, Pos, new Rectangle((int) Pos.X, (int) Pos.Y,  size,  size), Color.Aqua);

            //Pen p = new Pen(VColor, 2);
            //g.DrawEllipse(p, new Rectangle((int) leftCorner, (int) rightCorner, (int) size, (int) size));
            //g.DrawLine(p, (int) Pos.X, (int) Pos.Y, (int) Pos.X + (int)(Velocity.X * 2), (int)Pos.Y + (int)(Velocity.Y * 2));
        }
    }
}