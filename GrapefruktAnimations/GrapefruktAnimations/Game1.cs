using System.Linq;
using FlashAnimations.Import;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlashAnimations
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        private Animations animations;
        private AnimationPlayer player;
        private int index;
        private Textures sheets;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.Window.AllowUserResizing = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.animations = FlashImporter.LoadAnimations(Resource1.animations);
            this.sheets = FlashImporter.LoadSheets(Resource1.sheets);

            this.index = 0;
            this.playNext();
        }

        private void playNext()
        {
            this.index = (this.index + 1) % this.animations.Animation.Count();
            this.player = new AnimationPlayer(this.animations.Animation[index], this.sheets, playNext);
            this.player.LoadContent(this.Content);

            this.player.Scale = new Vector2(this.Window.ClientBounds.Height / 800f);
            this.player.Position = new Vector2(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height) / 2;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            this.player.Scale = new Vector2(this.Window.ClientBounds.Height / 800f);
            this.player.Position = new Vector2(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height) / 2;
            this.player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.player.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
