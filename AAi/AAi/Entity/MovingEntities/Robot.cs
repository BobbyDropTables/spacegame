using System;
using System.Collections.Generic;
using AAI.behaviour;
using AAI.Goals;
using AAI.View;
using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Entity.MovingEntities
{
    public class Robot : MovingEntity
    {
        //name of enitity for easy debugging 
        public string NAME;

        public new Texture2D Texture;

        public Robot(Vector2 pos, World w, string name) : base(pos, w)
        {
            Size = new Vector2(30,30);
            NAME      = name;
            Velocity  = new Vector2(0, 0);
            Behaviours = new List<SteeringBehaviour>
            {
                new WanderBehaviour(this, 90, 150),
                new WallAvoidance(this, 15),
                new SeperationBehaviour(this),
            };
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Texture = TextureStorage.Textures["Vacuum"];
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(
                             Texture,
                             new Rectangle(
                                           Pos.ToPoint(),
                                           Size.ToPoint()),
                             null,
                             Color.DarkGray,
                             (float) Math.Atan2(Heading.Y, Heading.X),
                             origin,
                             SpriteEffects.None,
                             0f);
            if (debug)
            {
               
                foreach (var behaviour in Behaviours)
                {
                    behaviour.DebugDraw(spriteBatch,10);
                }
            }
        }
    }
}