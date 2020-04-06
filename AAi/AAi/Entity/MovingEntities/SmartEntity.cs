using System;
using System.Collections.Generic;
using AAI.behaviour;
using AAI.FuzzyLogic;
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

        // Fuzzy members
        private Thirst _fuzzyThirst;
        private Hunger _fuzzyHunger;
        private Sleep _fuzzySleep;
        public SmartEntity(Vector2 pos, World w, string name) : base(pos, w)
        {
            /*
             * INIT FUZZY MODULES
             */
            _fuzzyThirst = new Thirst();
            _fuzzyHunger = new Hunger();
            _fuzzySleep = new Sleep();

            NAME = name;
            // random stats
            hunger    = MyWorld.Random.Next(0, 25);
            thirst    = MyWorld.Random.Next(0, 25);
            tiredness = MyWorld.Random.Next(0, 25);

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
            // OLD SIMPLE BEHAVIOUR
            //if stat is over 10 add goal to think else wander
            // if (hunger > 10)
            //     Think.SubGoals.Add(new GoEatGoal(this, MyWorld.food));
            // if (thirst > 10)
            //     Think.SubGoals.Add(new GoDrinkGoal(this, MyWorld.water));
            // if (tiredness > 10)
            //     Think.SubGoals.Add(new GoSleepGoal(this, MyWorld.bed));
            // if (hunger < 10 && thirst < 10 && tiredness < 10)
            //     Think.SubGoals.Add(new WanderGoal(this));

            // FUZZY LOGIC BEHAVIOUR
            // Calculate best goal with fuzzy logic
            double hungerDistance = Math.Abs((MyWorld.food.Pos.X - Pos.X) + (MyWorld.food.Pos.Y - Pos.Y));
            double hungerValue = _fuzzyHunger.CalculateDesirability(hungerDistance, hunger);

            double thirstDistance = Math.Abs((MyWorld.water.Pos.X - Pos.X) + (MyWorld.water.Pos.Y - Pos.Y));
            double thirstValue = _fuzzyThirst.CalculateDesirability(thirstDistance, thirst);

            double sleepDistance = Math.Abs((MyWorld.bed.Pos.X - Pos.X) + (MyWorld.bed.Pos.Y - Pos.Y));
            double sleepValue = _fuzzySleep.CalculateDesirability(sleepDistance, tiredness);

            // Wander if desirability of all values are low
            if (hungerValue < 25 && thirstValue < 25 && sleepValue < 25)
                Think.SubGoals.Add(new WanderGoal(this));
            else
            {
                // Apply distinct behaviour depending on value of desirability
                if (hungerValue > thirstValue && hungerValue > sleepValue)
                    Think.SubGoals.Add(new GoEatGoal(this, MyWorld.food));
                if (thirstValue > hungerValue && thirstValue > sleepValue)
                    Think.SubGoals.Add(new GoDrinkGoal(this, MyWorld.water));
                if (sleepValue > thirstValue && sleepValue > hungerValue)
                    Think.SubGoals.Add(new GoSleepGoal(this, MyWorld.bed));
            }
            
        }


        public override void Update()
        {
            thirst += 0.01f;
            hunger += 0.01f;
            tiredness += 0.002f;
            Think.Process();
            base.Update();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Texture = TextureStorage.Textures["Human"];
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(
                             Texture,
                             new Rectangle(
                                           Pos.ToPoint(),
                                           (Size * 2).ToPoint()),
                             null,
                             Color.White,
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
