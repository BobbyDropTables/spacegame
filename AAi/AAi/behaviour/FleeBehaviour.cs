using System;
using AAI.Entity.MovingEntities;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.behaviour
{
    internal class FleeBehaviour : SteeringBehaviour
    {
        private Vector2 result;
        private Vector2 target;
        public FleeBehaviour(MovingEntity me, Vector2 target,float force=1f) : base(me,force)
        {
            this.target = target;
        }

        public override Vector2 Calculate()
        {
            Vector2 tempPos         = ME.Pos;
            Vector2 DesiredVelocity = tempPos - target;
            DesiredVelocity.Normalize();
            DesiredVelocity *= ME.MaxSpeed;
            result = Vector2.Distance(ME.Pos, target) <= 2 ? new Vector2(0, 0) : DesiredVelocity - ME.Velocity;
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
        }
    }
}