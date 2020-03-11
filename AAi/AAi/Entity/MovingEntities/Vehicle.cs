﻿using Game.view;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.entity.MovingEntities
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
