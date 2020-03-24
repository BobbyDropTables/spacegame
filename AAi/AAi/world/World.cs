﻿using System.Collections.Generic;
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

        public GameMap gameMap { get; }

        public World(int w, int h)
        {
            Width  = w;
            Height = h;
            gameMap = new GameMap(w, h);

            populate();

            gameMap.FloodFill(walls);
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
                // Wall.CreateWall(this, new Vector2(12, 4))
            };
            foreach (Wall wall in Walls)
                walls.Add(wall);
        }

        // Vertex to recolor last vertex to yellow
        private bool isKeyReset = true;
        private bool PathingFinished = false;
        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                PathingFinished = false;
                // NavGraph.Reset();

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

                if (!PathingFinished)
                {
                    // Get the closest Vertex to current mouse click
                    var source = new Vector2((int)entities[0].Pos.X, (int)entities[0].Pos.Y);
                    var destination = new Vector2(mX, mY);

                    if (source != null && destination != null)
                    {
                        List<Edge> Path = gameMap.PathingPipeline(source, destination);
                        if(Path != null)
                            gameMap.commands = Path;

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
                gameMap.Render(spriteBatch);
            }

            Target.Render(spriteBatch);
        }
    }
}