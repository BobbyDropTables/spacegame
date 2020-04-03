using System;
using System.Collections.Generic;
using AAI.Entity.MovingEntities;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.Goals
{
    class Think : CompositeGoal
    {
        private int _y;

        public Think(SmartEntity smartEntity)
        {
            this.smartEntity = smartEntity;
            Name     = "Think";
            SubGoals = new List<CompositeGoal>();
        }

        public override void Activate()
        {
            SubGoals = new List<CompositeGoal>();
            State = Statusgoal.active;
            smartEntity.SelectGoal();
        }

        public override Statusgoal Process()
        {
            // Look for subgoals and execute if possible
            Statusgoal processState = ProcessAllSubgoals();

            // If everything is done -> activate again
            if (processState == Statusgoal.completed)
            {
                Activate();
            }
                

            return State;
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            _y = 0;
            DrawSubGoals(spriteBatch, this, 0);
        }

        private void DrawSubGoals(SpriteBatch spriteBatch, BaseGoal currentGoal, int depth)
        {
            _y += 24;
            var font = TextureStorage.Fonts["Font"];
            spriteBatch.DrawString(font, currentGoal.Name, new Vector2(smartEntity.Pos.X + depth * 6, smartEntity.Pos.Y +_y), currentGoal.State == Statusgoal.active ? Color.Green : Color.Black);

            var compositeGoal = currentGoal as CompositeGoal;

            if (compositeGoal?.SubGoals == null)
                return;

            foreach (var goal in compositeGoal.SubGoals)
            {
                DrawSubGoals(spriteBatch, goal, depth + 1);
            }
        }
    }
}