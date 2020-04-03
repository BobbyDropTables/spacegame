using System;
using System.Collections.Generic;
using AAI.behaviour;
using AAI.Goals;
using AAI.Pathing;
using AAI.View;
using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Entity.MovingEntities
{
    public class Vehicle : MovingEntity
    {
        public Texture2D   Texture;
        public bool        target { get; set; }
        public Queue<Edge> Path;
        public float hunger;
        private Think Think;
        public Vehicle(Vector2 pos, World w) : base(pos, w)
        {
            //StateMachine = new StateMachine<MovingEntity>(this);
            //StateMachine.Goal = new EatingGoal();
            Velocity = new Vector2(0, 0);
            target   = false;
            hunger = 0.0f;
            Think = new Think(this);
            Think.Activate();
            Behaviours = new List<SteeringBehaviour>
            {
                new WanderBehaviour(this,90,100),
                new WallAvoidance(this, 15),
            };

        }

        public void SelectGoal()
        {
            Think.SubGoals.Add(new GoEatGoal(this, MyWorld.food));
        }

        public override void Update()
        {
            Think.Process();
            base.Update();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (Path != null)
            {
                foreach (var current in Path)
                {
                    current.Draw(spriteBatch);
                    current.source.DrawVertex(spriteBatch);
                    current.destination.DrawVertex(spriteBatch);

                }
            }
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
            Think.DebugDraw(spriteBatch);
            //Behaviours[1].DebugDraw(spriteBatch, 10);
        }
    }
}