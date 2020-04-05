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

        public Vector2 RandomVector2inmap()
        {
            Vector2 Vector2 = new Vector2(Random.Next(20, 1260), Random.Next(20, 940));
            foreach (Wall wall in walls)
            {
                if (wall.IsWithin(Vector2))
                {
                    return RandomVector2inmap();
                }
            }

            return Vector2;
        }

        private void populate()
        {
            food = new Target(new Vector2(180, 60), this);
            water = new Target(new Vector2(1100, 800), this);
            bed = new Target(new Vector2(1200, 200), this);

            //create all walls
            List<Wall> Walls = new List<Wall>
            {
                // border walls
                new Wall(new Vector2(0, 0),
                         new Vector2(Width, 0 ),
                         new Vector2(Width, 15),
                         new Vector2(0, 15),
                         this, Color.Yellow
                        ),

                new Wall(new Vector2(Width, 0),
                         new Vector2(Width, Height ),
                         new Vector2(Width-15, Height),
                         new Vector2(Width-15, 0),
                         this, Color.Yellow
                        ),
                new Wall(new Vector2(0, Height-15),
                         new Vector2(Width, Height -15 ),
                         new Vector2(Width, Height),
                         new Vector2(0, Height),
                         this, Color.Yellow
                        ),

                new Wall(new Vector2(0, 0),
                         new Vector2(15, 0 ),
                         new Vector2(15, Height),
                         new Vector2(0, Height),
                         this, Color.Yellow
                        ),

                //other walls
                new Wall(new Vector2(0, 410),
                         new Vector2(365, 410 ),
                         new Vector2(365, 430),
                         new Vector2(0, 430),
                         this, Color.Yellow
                        ),
                new Wall(new Vector2(455, 410),
                         new Vector2(700, 410 ),
                         new Vector2(700, 430),
                         new Vector2(455, 430),
                         this, Color.Yellow
                        ),
                new Wall(new Vector2(590, 0),
                         new Vector2(605, 0 ),
                         new Vector2(605, 430),
                         new Vector2(590, 430),
                         this, Color.Yellow
                        ),
                new Wall(new Vector2(790, 410),
                         new Vector2(Width, 410 ),
                         new Vector2(Width, 430),
                         new Vector2(790, 430),
                         this, Color.Yellow
                        ),
                new Wall(new Vector2(825, 605),
                         new Vector2(Width, 605 ),
                         new Vector2(Width, 625),
                         new Vector2(825, 625),
                         this, Color.Yellow
                        ),
                new Wall(new Vector2(825, 605),
                         new Vector2(840, 605 ),
                         new Vector2(840, 680),
                         new Vector2(825, 680),
                         this, Color.Yellow
                        ),
                new Wall(new Vector2(825, 790),
                         new Vector2(840, 790 ),
                         new Vector2(840, Height),
                         new Vector2(825, Height),
                         this, Color.Yellow
                        ),

            };
            foreach (Wall wall in Walls)
                walls.Add(wall);

            //create robot
            for (int i = 0; i < 3; i++)
            {
                Robot v = new Robot(RandomVector2inmap(), this, i.ToString());
                MovingEntities.Add(v);
            }
            //create smart entities
            for (int i = 0; i < 5; i++)
            {
                SmartEntity v = new SmartEntity(RandomVector2inmap(), this, i.ToString());
                MovingEntities.Add(v);
            }
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
                walls.ForEach(e => e.Render(spriteBatch));
            }
            MovingEntities.ForEach(e => e.Render(spriteBatch));
            food.Render(spriteBatch, TextureStorage.Textures["Food"]);
            water.Render(spriteBatch, TextureStorage.Textures["Whiskey"]);
            bed.Render(spriteBatch, TextureStorage.Textures["Bed"]);
        }
    }
}