using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
#if WINDOWS_PHONE

using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;

#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using WP7QuestGame.Screens;
using WP7QuestGame.Components;
using System.IO.IsolatedStorage;


namespace WP7QuestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameLoop : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScreenManager screenManager;

#if WINDOWS_PHONE
        private bool HasBeenPromptedForMusic = false;
#else
        private bool HasBeenPromptedForMusic = true;
#endif

#if DEBUG
        FPSDisplay fps;
#endif
        public GameLoop()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if WINDOWS_PHONE

            //PhoneApplicationService.Current.Closing += GameDeactivated;
          //Cont
            
#endif
            
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Microsoft.Phone.Controls.

           screenManager = ScreenManager.getInstance();

            //String toto = Content.Load<String>("0.txt");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        protected override void LoadContent()
        {
            try
            {

                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);



#if WINDOWS_PHONE
                MessageBoxResult res = MessageBox.Show("This game contains some music. Do you wish to allow this application to play and adjust the background music ? \n\nSelecting Ok will allow the game to play some music", "Media Player Access", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                    GameSettings.AllowMusic = true;
                else if (res != MessageBoxResult.Cancel)
                    Exit();
                HasBeenPromptedForMusic = true;
#else
            GameSettings.AllowMusic = true;
#endif

                //Screen toto = new SurvivalModeScreen(this);
                //screenManager.pushScreen(new CreditScreen(this));
                //screenManager.pushScreen(toto);
                screenManager.pushScreen(new MenuScreen(this));

#if DEBUG
            SpriteFont font = Content.Load<SpriteFont>("fontFPS");

            fps = new FPSDisplay(font);
#endif

            }
            catch (Exception e)
            {
                UnexpectedExit();
            }
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
            try
            {

#if WINDOWS_PHONE
                CheckForBackButton();
#endif

                // TODO: Add your update logic here
                screenManager.Update(gameTime);

#if DEBUG
            fps.Update(gameTime);
#endif

                base.Update(gameTime);
            }
            catch (Exception e)
            {
                UnexpectedExit();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            try
            {

                GraphicsDevice.Clear(Color.Black);

                // TODO: Add your drawing code here
                screenManager.Draw();

#if DEBUG
            fps.Draw(this.spriteBatch);
#endif

                base.Draw(gameTime);
            }
            catch (Exception e)
            {
                UnexpectedExit();
            }
        }


        private void UnexpectedExit()
        {
#if WINDOWS_PHONE
            MessageBox.Show("Fries and furious has closed unexpectedly.", "An error occurred", MessageBoxButton.OK);
#endif
            this.Exit();
        }


#if WINDOWS_PHONE

    

        private void GameDeactivated(object sender, EventArgs e)
        {
            //this.Exit();
            ;
        }

        private void CheckForBackButton()
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (HasBeenPromptedForMusic == false)
                {
                    this.Exit();
                    return;
                }
                int count = screenManager.ScreenCount;

                if (count <= 1)
                    this.Exit();

                while (count > 1)
                {
                    screenManager.popScreen();
                    count--;
                }
            }
        }

#endif
    }
}
