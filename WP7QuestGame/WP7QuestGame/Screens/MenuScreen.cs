using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WP7QuestGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace WP7QuestGame.Screens
{
    class MenuScreen : Screen
    {
        private SpriteFont font;

        private Rectangle menuTarget = new Rectangle(282, 75, 236, 152);
        private Rectangle survivalTarget = new Rectangle(100, 250, 600, 50);
        private Rectangle instructionsTarget = new Rectangle(100, 310, 600, 50);
        private Rectangle creditsTarget = new Rectangle(100, 370, 600, 50);
        private Rectangle exitTarget = new Rectangle(100, 430, 600, 50);
        private Rectangle volumeTarget = new Rectangle(0, 380, 100, 100);

        private Vector2 scorePosition = new Vector2(250, 0);

        private bool exitPressed = false;
        private bool survivalPressed = false;
        private bool creditsPressed = false;
        private bool instructionsPressed = false;

        private bool manuallyStoppedMusic = false;

        public MenuScreen(GameLoop game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            font = Content.Load<SpriteFont>("fontFPS");

            Components.Add(new Background(Content.Load<Texture2D>("background2"), 70, false));

            if (GameSettings.AllowMusic && GameSettings.VolumeOn)
            {
                try
                {
                    MediaPlayer.Play(Content.Load<Song>("menusound"));
                    MediaPlayer.IsRepeating = true;
                }
                catch (Exception e) { ;}
            }

            base.Initialize();
        }

        public override void HandleInput()
        {
            if (GameSettings.AllowMusic && manuallyStoppedMusic && GameSettings.VolumeOn)
            {
                manuallyStoppedMusic = false;
                try
                {
                    MediaPlayer.Play(Content.Load<Song>("menusound"));
                    MediaPlayer.IsRepeating = true;
                }
                catch (Exception e) { ;}
            }

            TouchCollection tc = TouchPanel.GetState();

            if (tc.Count == 0)
            {
                exitPressed = false;
                survivalPressed = false;
                return;
            }

            

#if WINDOWS_PHONE
            #region EVENTS FOR PHONE

            foreach (TouchLocation tl in tc)
            {

                Rectangle touchRect = new Rectangle((int)tl.Position.X, (int)tl.Position.Y, 1, 1);

                if (tl.State == TouchLocationState.Pressed)
                {
                    if (exitTarget.Intersects(touchRect))
                        exitPressed = true;
                    else if (survivalTarget.Intersects(touchRect))
                        survivalPressed = true;
                    else if (creditsTarget.Intersects(touchRect))
                        creditsPressed = true;
                    else if (instructionsTarget.Intersects(touchRect))
                        instructionsPressed = true;
                    else if (volumeTarget.Intersects(touchRect) && GameSettings.AllowMusic)
                    {

                        if (GameSettings.VolumeOn)
                        {
                            GameSettings.VolumeOn = false;
                            MediaPlayer.Stop();
                        }
                        else
                        {
                            GameSettings.VolumeOn = true;
                            try
                            {
                                MediaPlayer.Play(Content.Load<Song>("menusound"));
                                MediaPlayer.IsRepeating = true;
                            }
                            catch (Exception e) { ;}
                        }
                    }
                }
                else if (tl.State == TouchLocationState.Released)
                {
                    if (exitTarget.Intersects(touchRect))
                    {
                        Game.Exit();
                        return;
                    }
                    else if (survivalTarget.Intersects(touchRect))
                    {
                        survivalPressed = false;
                        exitPressed = false;
                        creditsPressed = false;
                        instructionsPressed = false;
                        GameSettings.ScrollSpeed = GameSettings.DefaultScrollSpeed;
                        if (GameSettings.AllowMusic && GameSettings.VolumeOn)
                        {
                            MediaPlayer.Stop();
                            manuallyStoppedMusic = true;
                        }
                        ScreenManager.getInstance().pushScreen(new ScoreScreen(Game));
                        ScreenManager.getInstance().pushScreen(new SurvivalModeScreen(Game));
                        ScreenManager.getInstance().pushScreen(new GameStartScreen(Game));
                    }
                    else if (creditsTarget.Intersects(touchRect))
                    {
                        survivalPressed = false;
                        exitPressed = false;
                        creditsPressed = false;
                        instructionsPressed = false;
                        ScreenManager.getInstance().pushScreen(new CreditScreen(Game));
                    }
                    else if (instructionsTarget.Intersects(touchRect))
                    {
                        survivalPressed = false;
                        exitPressed = false;
                        creditsPressed = false;
                        instructionsPressed = false;
                        ScreenManager.getInstance().pushScreen(new InstructionScreen(Game));
                    }
                }
            }
            #endregion
#else
            
            #region EVENTS FOR WINDOWS


            Rectangle touchRect = new Rectangle((int)Mouse.GetState().X, (int)Mouse.GetState().Y, 1, 1);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (exitTarget.Intersects(touchRect))
                    exitPressed = true;
                else if (survivalTarget.Intersects(touchRect))
                    survivalPressed = true;
                else if (creditsTarget.Intersects(touchRect))
                    creditsPressed = true;
                else if (instructionsTarget.Intersects(touchRect))
                    instructionsPressed = true;
                else if (volumeTarget.Intersects(touchRect))
                {
                    if (GameSettings.VolumeOn)
                    {
                        GameSettings.VolumeOn = false;
                        MediaPlayer.Stop();
                    }
                    else
                    {
                        GameSettings.VolumeOn = true;
                        MediaPlayer.Play(Content.Load<Song>("menusound"));
                        MediaPlayer.IsRepeating = true;
                    }
                }
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (exitTarget.Intersects(touchRect))
                {
                    Game.Exit();
                    return;
                }
                else if (survivalTarget.Intersects(touchRect))
                {
                    survivalPressed = false;
                    exitPressed = false;
                    creditsPressed = false;
                    instructionsPressed = false;
                    GameSettings.ScrollSpeed = GameSettings.DefaultScrollSpeed;
                    MediaPlayer.Stop();
                    manuallyStoppedMusic = true;
                    ScreenManager.getInstance().pushScreen(new SurvivalModeScreen(Game));
                    ScreenManager.getInstance().pushScreen(new GameStartScreen(Game));
                }
                else if (creditsTarget.Intersects(touchRect))
                {
                    survivalPressed = false;
                    exitPressed = false;
                    creditsPressed = false;
                    instructionsPressed = false;
                    ScreenManager.getInstance().pushScreen(new CreditScreen(Game));
                }
                else if (instructionsTarget.Intersects(touchRect))
                {
                    survivalPressed = false;
                    exitPressed = false;
                    creditsPressed = false;
                    instructionsPressed = false;
                    ScreenManager.getInstance().pushScreen(new InstructionScreen(Game));
                }
            }

            #endregion
#endif

            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();

            SpriteBatch.Begin();

            SpriteFont sf = Content.Load<SpriteFont>("ScoreFont");
            Vector2 size = sf.MeasureString("High Score : " + ScoreManager.getInstance().BestScore);
            scorePosition.X = (800 - size.X);

            SpriteBatch.DrawString(Content.Load<SpriteFont>("ScoreFont"), "High Score : " + ScoreManager.getInstance().BestScore, scorePosition, Color.Gray);
            SpriteBatch.Draw(Content.Load<Texture2D>("titlemenu"), menuTarget, Color.White);
            SpriteBatch.Draw(Content.Load<Texture2D>("instructionsbutton"), instructionsTarget, (instructionsPressed ? Color.Purple : Color.White));
            SpriteBatch.Draw(Content.Load<Texture2D>("survivalbutton"), survivalTarget, (survivalPressed ? Color.Blue : Color.White));
            SpriteBatch.Draw(Content.Load<Texture2D>("creditsbutton"), creditsTarget, (creditsPressed ? Color.Green : Color.White));
            SpriteBatch.Draw(Content.Load<Texture2D>("exitbutton"), exitTarget, (exitPressed ? Color.Red : Color.White));

            if (GameSettings.VolumeOn)
                SpriteBatch.Draw(Content.Load<Texture2D>("volumeon"), volumeTarget, Color.White);
            else
                SpriteBatch.Draw(Content.Load<Texture2D>("volumeoff"), volumeTarget, Color.White);

            SpriteBatch.End();


        }
    }
}
