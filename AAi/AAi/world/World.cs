using System;
using System.Collections.Generic;
using System.Linq;
using AAI.behaviour;
using AAI.Entity;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using AAI.Pathing;
using AAI.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AAI.world
{
    public class World
    {
        public Target food { get; set; }
        public Target water { get; set; }
        public Target bed { get; set; }
        public           int                  Width  { get; set; }
        public           int                  Height { get; set; }
        public  List<MovingEntity> MovingEntities  = new List<MovingEntity>();
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
            food = new Target(new Vector2(300, 300), this);
            water = new Target(new Vector2(1100, 800), this);
            bed = new Target(new Vector2(1100, 100), this);

            //create robot
            for (int i = 0; i < 50; i++)
            {
                Robot v = new Robot(new Vector2(Random.Next(20, 1260), Random.Next(20, 940)), this, i.ToString());
                MovingEntities.Add(v);
            }
            //create smart entities
            for (int i = 0; i < 3; i++)
            {
                SmartEntity v = new SmartEntity(new Vector2(Random.Next(20, 1260), Random.Next(20, 940)), this, i.ToString());
                MovingEntities.Add(v);
            }

            //create all walls
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

                // border walls
                new Wall(new Vector2(0, 0),
                         new Vector2(Width-10, 0 ),
                         new Vector2(Width-10, 10),
                         new Vector2(0, 10),
                         this, Color.Blue
                        ),

                new Wall(new Vector2(Width, 0),
                         new Vector2(Width, Height ),
                         new Vector2(Width -10, Height),
                         new Vector2(Width-10, 0),
                         this, Color.Blue
                        ),
                new Wall(new Vector2(10, Height-10),
                         new Vector2(Width -10, Height-10 ),
                         new Vector2(Width -10, Height),
                         new Vector2(10, Height),
                         this, Color.Blue
                        ),

                new Wall(new Vector2(10, 0),
                         new Vector2(10, Height ),
                         new Vector2(0, Height),
                         new Vector2(0, 0),
                         this, Color.Blue
                        ),

            };
            foreach (Wall wall in Walls)
                walls.Add(wall);
        }

        // Vertex to recolor last vertex to yellow
        private bool isKeyReset = true;
        private bool isKeyeReset = false;
        private bool PathingFinished = false;
        public void Update()
        {
            //MouseState mouseState = Mouse.GetState();
            //if (mouseState.LeftButton == ButtonState.Pressed)
            //{
            //    PathingFinished = false;
            //    // NavGraph.Reset();

            //    int mX = mouseState.X;
            //    int mY = mouseState.Y;

            //    // Make sure mouse clicks are within bounds
            //    if (mX > Width)
            //        mX = Width;
            //    if (mX < 0)
            //        mX = 0;
            //    if (mY > Height)
            //        mY = Height;
            //    if (mY < 0)
            //        mY = 0;

            //    Target.Pos = new Vector2(mX, mY);

            //    if (!PathingFinished)
            //    {
            //        // Get the closest Vertex to current mouse click
            //        var source = new Vector2((int)entities[0].Pos.X, (int)entities[0].Pos.Y);
            //        var destination = new Vector2(mX, mY);

            //        if (source != null && destination != null)
            //        { 
            //            var Path = gameMap.PathingPipeline(source, destination, walls);
            //            var test = (Robot) entities[0];
            //            test.Path = Path;
            //            if(Path != null)
            //                gameMap.commands = Path;

            //            PathingFinished = true;
            //        }
            //    }
                
            //}

            // Poll for current keyboard goal
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.E) && isKeyeReset)
            {
                isKeyeReset = false;
                foreach (var me in MovingEntities)
                {
                    me.debug = !me.debug;
                }

            }

            if (state.IsKeyUp(Keys.E) && !isKeyeReset)
                isKeyeReset = true;

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.G) && isKeyReset)
            {
                isKeyReset = false;
                DrawGraph = !DrawGraph;

            }

            if (state.IsKeyUp(Keys.G) && !isKeyReset)
                isKeyReset = true;

            foreach (MovingEntity me in MovingEntities)
            {
                me.Update();
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            if (DrawGraph)
            {
                gameMap.Render(spriteBatch);
            }
            walls.ForEach(e => e.Render(spriteBatch));
            MovingEntities.ForEach(e => e.Render(spriteBatch));
            food.Render(spriteBatch, TextureStorage.Textures["Food"]);
            water.Render(spriteBatch, TextureStorage.Textures["Whiskey"]);
            bed.Render(spriteBatch, TextureStorage.Textures["Bed"]);
        }
    }
}