using Game.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SteeGameringCS.behaviour;
using System.Collections.Generic;
using Game.entity.MovingEntities;
using Game.entity.staticEntities;

namespace Game
{
    public class World
    {
        private List<BaseGameEntity> entities = new List<BaseGameEntity>();
        public Target Target { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public World(int w, int h)
        {
            Width = w;
            Height = h;
            populate();
        }

        private void populate()
        {
            Target = new Target(new Vector2(100, 60), this);

            Vehicle v = new Vehicle(new Vector2(120,70), this);
            entities.Add(v);

            
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
               Target.Pos = new Vector2(mouseState.X, mouseState.Y);
            }
            foreach (MovingEntity me in entities)
            {
                me.SB = new ArriveBehaviour(me);
                me.Update();
            }  
        }

        public void Render(SpriteBatch spriteBatch)
        {
            entities.ForEach(e => e.Render(spriteBatch));
            Target.Render(spriteBatch);
        }
    }
}
