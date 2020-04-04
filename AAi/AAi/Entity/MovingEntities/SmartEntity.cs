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
    public class SmartEntity : MovingEntity
    {
        //stats for Goals
        public float hunger;

        //name of enitity for easy debugging 
        public string NAME;

        public new Texture2D Texture;

        //Brain For Goals
        private readonly Think Think;
        public float thirst;
        public float tiredness;

        public SmartEntity(Vector2 pos, World w, string name) : base(pos, w)
        {
            NAME = name;
            // random stats
            hunger    = MyWorld.Random.Next(0, 10);
            thirst    = MyWorld.Random.Next(0, 10);
            tiredness = MyWorld.Random.Next(0, 10);

            Velocity = new Vector2(0, 0);
            Think = new Think(this);
            Think.Activate();
            Behaviours = new List<SteeringBehaviour>
            {
                new WanderBehaviour(this, 90, 100),
                new WallAvoidance(this, 15)
            };
        }

        public void SelectGoal()
        {
            //if stat is over 10 add goal to think else wander
            if (hunger > 10)
                Think.SubGoals.Add(new GoEatGoal(this, MyWorld.food));
            if (thirst > 10)
                Think.SubGoals.Add(new GoDrinkGoal(this, MyWorld.water));
            if (tiredness > 10)
                Think.SubGoals.Add(new GoSleepGoal(this, MyWorld.bed));
            if (hunger < 10 && thirst < 10 && tiredness < 10)
                Think.SubGoals.Add(new WanderGoal(this));
        }

        public override void Update()
        {
            thirst += 0.005f;
            hunger += 0.005f;
            tiredness += 0.0005f;
            Think.Process();
            base.Update();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Texture = TextureStorage.Textures["Arrow"];
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
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
            if (debug)
            {
                string Stats = $"T:{thirst}  H:{hunger}  S:{tiredness}";
                Think.DebugDraw(spriteBatch);
                foreach (var behaviour in Behaviours)
                {
                    behaviour.DebugDraw(spriteBatch, 10);
                }
                SpriteFont font = TextureStorage.Fonts["Font"];
                spriteBatch.DrawString(font, Stats, new Vector2(Pos.X + 6, Pos.Y), Color.White);
            }
        }
    }
}
