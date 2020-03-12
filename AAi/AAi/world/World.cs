using System.Collections.Generic;
using AAI.Entity;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SteeGameringCS.behaviour;

namespace AAI.world
{
    public class World
    {
        public           Target               Target { get; set; }
        public           int                  Width  { get; set; }
        public           int                  Height { get; set; }
        private readonly List<BaseGameEntity> entities  = new List<BaseGameEntity>();
        private readonly List<BaseGameEntity> walls     = new List<BaseGameEntity>();

        public World(int w, int h)
        {
            Width  = w;
            Height = h;
            populate();
        }

        private void populate()
        {
            Target = new Target(new Vector2(100, 60), this);

            Vehicle v = new Vehicle(new Vector2(120, 70), this);
            entities.Add(v);
            List<Wall> Walls = new List<Wall>
            {
                new Wall(new Vector2(0, 0), this, new Vector2(Width, 0), 20, Color.Black),
                new Wall(new Vector2(Width, Height), this, new Vector2(Width, 0), 20, Color.Black),
                new Wall(new Vector2(0, Height), this, new Vector2(Width, Height), 20, Color.Black),
                new Wall(new Vector2(0, Height), this, new Vector2(0, 0), 20, Color.Black),
                new Wall(new Vector2(100,100), this,new Vector2(200,100),20,Color.Purple)
            };
            foreach (Wall wall in Walls)
                walls.Add(wall);
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
                Target.Pos = new Vector2(mouseState.X, mouseState.Y);
            foreach (MovingEntity me in entities)
            {
                me.SB = new ArriveBehaviour(me);
                me.Update();
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            walls.ForEach(e => e.Render(spriteBatch));
            entities.ForEach(e => e.Render(spriteBatch));
            Target.Render(spriteBatch);
        }
    }
}