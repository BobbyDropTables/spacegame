using System.Collections.Generic;
using AAI.behaviour;
using AAI.View;
using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Entity.MovingEntities
{
    class Vehicle : MovingEntity
    {
        public          Texture2D Texture;
        public readonly Rectangle DrawRectangle = new Rectangle(200,200,200,200);
        public bool target { get; set; }

        public Vehicle(Vector2 pos, World w) : base(pos, w)
        {
            Velocity = new Vector2(0, 0);
            target = false;
            Behaviours = new List<SteeringBehaviour>
            {
                new ArriveBehaviour(this),
                //new ObjectAvoidance(this, 50),
            };
        }

		public override void Render(SpriteBatch spriteBatch)
        {
            Texture = TextureStorage.Textures["Arrow"];
            spriteBatch.Draw(Texture,
                           new Rectangle((int) Pos.X, (int) Pos.Y, (int) Scale, (int) Scale),
                           null,
                           Color.White);
        }
	}
}
