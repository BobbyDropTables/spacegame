using System;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.behaviour
{
    internal class ArriveBehaviour : SteeringBehaviour
    {
        private Vector2 desiredVelocity;
        private Vector2 Target;
        public ArriveBehaviour(MovingEntity me, Vector2 target, float force =1f) : base(me, force)
        {
            Target = target;
        }

        public override Vector2 Calculate()
        {
            //calculate the distance to the target position
            Vector2 toTarget = Target - ME.Pos;
            float dist = toTarget.Length();

            if (dist > 0)
            {
                float speed = dist / 50;
                speed = Math.Min(speed, ME.MaxSpeed);
                desiredVelocity = toTarget * speed / dist;
                return desiredVelocity - ME.Velocity * Force;
            }
            return new Vector2(0, 0);
        }

        public override void DebugDraw(SpriteBatch spriteBatch, float scale)
        {
            
            var Start = ME.Pos;
            var End = ME.Pos + desiredVelocity * scale;
            Vector2 edge = End - Start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);
            var Texture = TextureStorage.Textures["Line"];
            var origin = new Vector2(0);
            spriteBatch.Draw(Texture,
                             new Rectangle(
                                           (int)Start.X,
                                           (int)Start.Y,
                                           (int)edge.Length(),
                                           1),
                             null,
                             Color.Violet,
                             angle,
                             new Vector2(0, 0.5f),
                             SpriteEffects.None,
                             0);
        }
    }
}