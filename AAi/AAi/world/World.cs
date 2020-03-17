using System;
using System.Collections.Generic;
using System.Linq;
using AAI.behaviour;
using AAI.Entity;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AAI.world
{
    public class World
    {
        public           Target               Target { get; set; }
        public           int                  Width  { get; set; }
        public           int                  Height { get; set; }
        public  List<BaseGameEntity> Entities  = new List<BaseGameEntity>();
        public Random Random = new Random();

        public World(int w, int h)
        {
            Width  = w;
            Height = h;
            populate();
        }

        private void populate()
        {
            Target = new Target(new Vector2(500, 300), this);

            Vehicle v = new Vehicle(new Vector2(300, 300), this);
            Entities.Add(v);
            v = new Vehicle(new Vector2(400, 400), this);
            Entities.Add(v);
            v = new Vehicle(new Vector2(600, 100), this);
            Entities.Add(v);
            v = new Vehicle(new Vector2(1000, 800), this);
            Entities.Add(v);
            v = new Vehicle(new Vector2(700, 300), this);
            Entities.Add(v);
            List<Wall> Walls = new List<Wall>
            {
                new Wall(new Vector2(0, 0), this, new Vector2(Width, 0), 20, Color.Black),
                new Wall(new Vector2(Width, 0), this, new Vector2(Width, Height), 20, Color.Black),
                new Wall(new Vector2(0, 0), this, new Vector2(0, Height), 20, Color.Black),
                new Wall(new Vector2(0, Height), this, new Vector2(Width, Height), 20, Color.Black),
                new Wall(new Vector2(500,300), this,new Vector2(500,600),20,Color.Purple)
            };
            foreach (Wall wall in Walls)
                Entities.Add(wall);
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
                Target.Pos = new Vector2(mouseState.X, mouseState.Y);
            var melist = Entities.Where(entity => entity is MovingEntity).ToList();
            foreach (MovingEntity me in melist)
            {
                me.Update();
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            Entities.ForEach(e => e.Render(spriteBatch));
            Target.Render(spriteBatch);
        }
    }
}