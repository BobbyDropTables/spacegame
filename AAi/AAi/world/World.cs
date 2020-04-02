using System;
using System.Collections.Generic;
using System.Linq;
using AAI.behaviour;
using AAI.Entity;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using AAI.Pathing;
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
        public Random Random = new Random();
        public bool DrawGraph { get; set; }
        public  List<BaseGameEntity> walls     = new List<BaseGameEntity>();
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
            Target = new Target(new Vector2(500, 300), this);
            for (int i = 0; i < 1; i++)
            {
                Vehicle v = new Vehicle(new Vector2(Random.Next(20, 1260), Random.Next(20, 940)), this);
                entities.Add(v);
            }
            

            List<Wall> Walls = new List<Wall>
            {
                new Wall(new Vector2(520, 520), 
                    new Vector2(520 + (gameMap.TILE_SIZE * 4), 520 ), 
                    new Vector2(520 + (gameMap.TILE_SIZE * 4), 520 + gameMap.TILE_SIZE),
                    new Vector2(520, 520 + gameMap.TILE_SIZE),
                    this, Color.Blue
                    ),
                new Wall(new Vector2(0, 0),
                    new Vector2(0 + (gameMap.TILE_SIZE * 4), 0 ),
                    new Vector2(0 + (gameMap.TILE_SIZE * 4), 0 + gameMap.TILE_SIZE),
                    new Vector2(0, 0 + gameMap.TILE_SIZE),
                    this, Color.Blue
                ),
                new Wall(new Vector2(0, 0),
                         new Vector2(Width, 0 ),
                         new Vector2(Width, 10),
                         new Vector2(0, 10),
                         this, Color.Blue
                        ),
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
                        var Path = gameMap.PathingPipeline(source, destination, walls);
                        var test = (Vehicle) entities[0];
                        test.Path = Path;
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