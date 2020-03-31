using System;
using System.Security.Cryptography;
using AAI.Entity.MovingEntities;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.behaviour
{
    internal class WanderBehaviour : SteeringBehaviour
    {
        private readonly float wanderDistance;
        private readonly float wanderRadius;
        private Vector2 result;
        private Vector2 circlePos;
        private Vector2 target;

        public WanderBehaviour(MovingEntity me, float radius, float distance) : base(me)
        {
            wanderRadius   = radius;
            wanderDistance = distance;
        }

        public override Vector2 Calculate()
        {
            var randomdeg = ME.MyWorld.Random.Next(0, 360);
            float  sin    = (float) Math.Sin(MathHelper.ToDegrees(randomdeg));
            float  cos    = (float) Math.Cos(MathHelper.ToDegrees(randomdeg));

            float tx = wanderRadius;
            int   ty = 0;

            Vector2 rotate = new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);

            circlePos = ME.Pos      + ME.Velocity * wanderDistance;
            target    = circlePos  + rotate;
            result = Vector2.Normalize(target - ME.Pos);
            return result;
        }

        public override void DebugDraw(SpriteBatch spriteBatch, float scale)
        {
            var Start = ME.Pos;
            var End   = ME.Pos + result * scale;
            Vector2 edge = End - Start;
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
                             Color.Violet,
                             angle,
                             new Vector2(0, 0.5f),
                             SpriteEffects.None,
                             0);
            Vector2 size = new Vector2(wanderRadius + wanderRadius);
            //// Draw border circle if neccesairy.
            //spriteBatch.Draw(Texture,
            //                 new Rectangle(
            //                               new Point((int)(circlePos.X - size.X / 2 - 2), (int)(circlePos.Y - size.Y / 2 - 2)),
            //                               new Point((int)(size.X                + 2 * 2), (int)(size.Y            + 2 * 2))),
            //                 null, Color.Black);

            //// Draw inner circle.
            //spriteBatch.Draw(Texture,
            //                 new Rectangle(
            //                               new Point((int)(circlePos.X - size.X / 2), (int)(circlePos.Y - size.Y / 2)),
            //                               new Point((int)size.X, (int)size.Y)),
            //                 null, Color.BlueViolet);
        }
    }
}