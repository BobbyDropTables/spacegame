using System;
using AAI.Entity.MovingEntities;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.behaviour
{
    internal class WanderBehaviour : SteeringBehaviour
    {
        private readonly float   wanderDistance;
        private readonly float   wanderRadius;
        private          Vector2 result;

        public WanderBehaviour(MovingEntity me, float radius, float distance, float force = 1f) : base(me, force)
        {
            wanderRadius   = radius;
            wanderDistance = distance;
        }

        public override Vector2 Calculate()
        {
            int   randomdeg = ME.MyWorld.Random.Next(0, 360);
            float sin       = (float) Math.Sin(MathHelper.ToDegrees(randomdeg));
            float cos       = (float) Math.Cos(MathHelper.ToDegrees(randomdeg));

            float tx = wanderRadius;
            int   ty = 2;

            Vector2 rotate = new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
            Vector2 circlePos = ME.Pos    + ME.Velocity * wanderDistance;
            Vector2 target    = circlePos + rotate;
            result = Vector2.Normalize(target - ME.Pos);
            return result * Force;
        }

        public override void DebugDraw(SpriteBatch spriteBatch, float scale)
        {
            Vector2 Start = ME.Pos;
            Vector2 End   = ME.Pos + result * scale;
            Vector2 edge  = End    - Start;
            // calculate angle to rotate line
            float angle =
                (float) Math.Atan2(edge.Y, edge.X);
            Texture2D Texture = TextureStorage.Textures["Line"];
            Vector2   origin  = new Vector2(0);
            spriteBatch.Draw(Texture,
                             new Rectangle(
                                           (int) Start.X,
                                           (int) Start.Y,
                                           (int) edge.Length(),
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