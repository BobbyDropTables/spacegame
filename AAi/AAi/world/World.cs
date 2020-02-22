using System.Collections.Generic;
using entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SteeringCS.behaviour;

namespace World
{
    internal class World
    {
        public           Vehicle            Target { get; set; }
        public           int                Width  { get; set; }
        public           int                Height { get; set; }
        private  Texture2D          dobberTexture2D;
        private readonly List<MovingEntity> entities = new List<MovingEntity>();
        private  Texture2D          fishTexture2D;

        public World(int w, int h, Texture2D fishTexture2D, Texture2D dobberTexture2D)
        {
            Width  = w;
            Height = h;
            this.fishTexture2D   = fishTexture2D;
            this.dobberTexture2D = dobberTexture2D;
            populate();
        }

        private void populate()
        {
            var v = new Vehicle(new Vector2(120, 70), this, fishTexture2D);
            v.target = false;
            entities.Add(v);

            Target        = new Vehicle(new Vector2(100, 60), this, dobberTexture2D);
            Target.target = true;
            Target.Pos    = new Vector2(100, 40);
        }

        public void Update(GameTime timeElapsed)
        {
            foreach (var me in entities)
            {
                me.SB = new ArriveBehaviour(me); // restore later
                me.Update(timeElapsed);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            entities.ForEach(e => e.Draw(spriteBatch));
            Target.Draw(spriteBatch);
        }
    }
}