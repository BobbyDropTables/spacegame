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
        public  List<BaseGameEntity> entities  = new List<BaseGameEntity>();

        public World(int w, int h)
        {
            Width  = w;
            Height = h;
            populate();
        }

        internal object Where(Func<object, bool> p)
        {
            throw new NotImplementedException();
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
                entities.Add(wall);
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
                Target.Pos = new Vector2(mouseState.X, mouseState.Y);
            var melist = entities.Where(entity => entity is MovingEntity).ToList();
            foreach (MovingEntity me in melist)
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