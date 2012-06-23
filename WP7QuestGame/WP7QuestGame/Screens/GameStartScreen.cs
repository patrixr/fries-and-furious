using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using WP7QuestGame.Components.Particles;



namespace WP7QuestGame.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameStartScreen : Screen
    {
        private SpriteFont font;
        private int timeElapsed = 0;
        private int countdown = 3;
        //private ParticleEmitter _particleEmitter;

        public GameStartScreen(GameLoop game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            font = Content.Load<SpriteFont>("TimerFont");

            /*_particleEmitter = new ParticleEmitter(new RainbowParticleGenerator(Content.Load<Texture2D>("circle")), new Vector2(700, 240), GameSettings.ParticleGenerationCount);
            _particleEmitter.ZIndex = 2;
            _particleEmitter.Start();
            this.Components.Add(_particleEmitter);*/
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (countdown == 0)
            {
                ScreenManager.getInstance().popScreen();
                return;
            }

            timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (timeElapsed >= 1000)
            {
                timeElapsed = timeElapsed - 1000;
                countdown--;
            }

            base.Update(gameTime);
        }

        public override void HandleInput()
        {
            TouchCollection tc = TouchPanel.GetState();
            foreach (TouchLocation tl in tc)
            {
                if (tl.State == TouchLocationState.Released)
                {
                    ScreenManager.getInstance().popScreen();
                    break;
                }
            }
        }

        public override void Draw()
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(Content.Load<Texture2D>("gamestarting"), Vector2.Zero, Color.White); 
            SpriteBatch.DrawString(font, countdown.ToString(), new Vector2(350, 300), Color.White);


            SpriteBatch.End();

            base.Draw();
        }
    }
}
