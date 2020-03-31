using System;
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
        public bool target { get; set; }
        public Vehicle(Vector2 pos, World w) : base(pos, w)
        {
            //StateMachine = new StateMachine<MovingEntity>(this);
            //StateMachine.State = new Attack();
            Velocity = new Vector2(0, 0);
            target = false;
            Behaviours = new List<SteeringBehaviour>
            {
                new PathFollowing(this),
                //new ArriveBehaviour(this),
                //new WanderBehaviour(this,90,100),
                //new WallAvoidance(this, 15),
            };

        }

		public override void Render(SpriteBatch spriteBatch)
        {
            Texture = TextureStorage.Textures["Arrow"];
            var origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(
                             Texture,
                             new Rectangle(
                                           Pos.ToPoint(),
                                           Size.ToPoint()),
                             null,
                             Color.Aqua,
                             (float)Math.Atan2(Heading.Y, Heading.X),
                             origin,
                             SpriteEffects.None,
                             0f);
        }
    }
}
