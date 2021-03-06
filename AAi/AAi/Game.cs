﻿using AAI.View;
using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AAI
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public const     int                   WindowWidth  = 1280;
        public const     int                   WindowHeight = 960;
        private readonly GraphicsDeviceManager _graphics;
        private          SpriteBatch           spriteBatch;
        private          TextureStorage        textureStorage;
        private FrameCounter _frameCounter = new FrameCounter();
        private readonly World world;
        private SpriteFont font;
        public Game()
        {
            _graphics                           = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.PreferredBackBufferWidth  = WindowWidth;
            Content.RootDirectory               = "Content";
            IsMouseVisible                      = true;
            world                               = new World(WindowWidth, WindowHeight);
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            textureStorage = new TextureStorage(Content);

            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureStorage.LoadTextures();
            font = Content.Load<SpriteFont>("Font");
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (this.IsActive)
            {
                world.Update();
            }
            
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

            

            // Clear the screen and add a background color.
            spriteBatch.Begin();
            spriteBatch.Draw(TextureStorage.Textures["Background"], new Rectangle(0, 0, 1280, 960), Color.White);
            // Draw all the Entities.
            world.Render(spriteBatch);
            //draw fps
            spriteBatch.DrawString(font, fps, new Vector2(1, 1), Color.Red);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}