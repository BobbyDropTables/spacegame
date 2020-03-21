using System.Collections.Generic;
using AAI.Entity;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using AAI.Pathing;
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
        public bool DrawGraph { get; set; }
        private readonly List<BaseGameEntity> entities  = new List<BaseGameEntity>();
        private readonly List<BaseGameEntity> walls     = new List<BaseGameEntity>();
        private Graph NavGraph;

        public World(int w, int h)
        {
            Width  = w;
            Height = h;
            populate();

            NavGraph = new Graph(w, h);
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

        // Vertex to recolor last vertex to yellow
        private Vertex[] lastColored;
        private bool isKeyReset = true;
        private bool PathingFinished = false;
        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                PathingFinished = false;
                NavGraph.Reset();

                int mX = mouseState.X;
                int mY = mouseState.Y;

                // Make sure mouse clicks are within bounds
                if (mX > Width)
                    mX = Width;
                if (mX < 0)
                    mX = 0;
                if (mY > Height)
                    mY = Height;
                if (mY < 0)
                    mY = 0;

                Target.Pos = new Vector2(mX, mY);

                // Color last selected vertex to yellow
                if (lastColored != null)
                {
                    lastColored[0].color = Color.Yellow;
                    lastColored[1].color = Color.Yellow;
                }
                else
                {
                    lastColored = new Vertex[2];
                }

                if (!PathingFinished)
                {
                    NavGraph.Reset();
                    // Get the closest Vertex to current mouse click
                    var source = NavGraph.ClosestVertexToPosition((int)entities[0].Pos.X, (int)entities[0].Pos.Y);
                    var destination = NavGraph.ClosestVertexToPosition(mX, mY);

                    if (source != null && destination != null)
                    {
                        // Color current Vertex to Red
                        source.color = Color.Red;
                        destination.color = Color.Red;

                        lastColored[0] = source;
                        lastColored[1] = destination;

                        List<Vertex> Path = NavGraph.A_Star(source, destination);
                        Graph.DrawPath(Path);
                        PathingFinished = true;
                    }
                }
                
            }

            // Poll for current keyboard state
            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.G) && isKeyReset)
            {
                isKeyReset = false;
                DrawGraph = !DrawGraph;

            }

            if (state.IsKeyUp(Keys.G) && !isKeyReset)
                isKeyReset = true;

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
            if (DrawGraph)
            {
                NavGraph.Draw(spriteBatch);
            }

            Target.Render(spriteBatch);
        }
    }
}