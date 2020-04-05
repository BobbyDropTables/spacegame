using System;
using System.Collections.Generic;
using AAI.Entity;
using AAI.Entity.MovingEntities;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.behaviour
{
    class SeperationBehaviour : SteeringBehaviour
    {

        private Vector2 steeringForce;

        public SeperationBehaviour(MovingEntity me, float force = 5f) : base(me, force)
        {
           
        }

        public override Vector2 Calculate()
        {
            
            List<MovingEntity> neighbors     = ME.MyWorld.MovingEntities;
            steeringForce = new Vector2();
            foreach (MovingEntity neighbor in neighbors)
            {
                // Check if the neighbor isn't this entity.
                if (neighbor != ME && neighbor.IsTagged)
                {
                    Vector2 toAgent = ME.Pos - neighbor.Pos;
                    steeringForce += Vector2.Normalize(toAgent) / toAgent.Length();
                }
            }
            return steeringForce * Force;
        }

        public override void DebugDraw(SpriteBatch spriteBatch, float scale)
        {
            var     Start = ME.Pos;
            var     End   = ME.Pos + steeringForce * 1000;
            Vector2 edge  = End    - Start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);
            var Texture = TextureStorage.Textures["Line"];
            var origin  = new Vector2(0);
            spriteBatch.Draw(Texture,
                             new Rectangle(
                                           (int)Start.X,
                                           (int)Start.Y,
                                           (int)edge.Length(),
                                           1),
                             null,
                             Color.Green,
                             angle,
                             new Vector2(0, 0.5f),
                             SpriteEffects.None,
                             0f);
        }
    }
}