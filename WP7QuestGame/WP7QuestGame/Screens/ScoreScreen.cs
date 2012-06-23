using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if WINDOWS_PHONE

using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Input.Touch;

#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WP7QuestGame.Components;
using WP7QuestGame.Components.Particles;

namespace WP7QuestGame.Screens
{
    class ScoreScreen : Screen
    {
        private int timeout = 0;

        SpriteFont fontscore;
        SpriteFont fonttext;

        private static List<String> Comments = new List<string>()
        {
            "You shall not pass !",
            "I see what you did there ;)",
            "One does not simply run on the ceiling !",
            "Close enough !",
            "Not bad !",
            "Well played :)",
            "Oh stop it you :3",
            "So Fast !",
            "Fast as hell !",
            "\\o/"
        };

        public ScoreScreen(GameLoop game)
            : base(game)
        {
            fontscore = Content.Load<SpriteFont>("LargeFont");
            fonttext = Content.Load<SpriteFont>("ScoreFont");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            timeout += gameTime.ElapsedGameTime.Milliseconds;

            if (timeout >= 3000)
            {
                ScreenManager.getInstance().popScreen();
                timeout = 0;
                return;
            }

            base.Update(gameTime);
        }

        public override void HandleInput()
        {
#if WINDOWS_PHONE
            TouchCollection tc = TouchPanel.GetState();
            foreach (TouchLocation tl in tc)
            {
                if (tl.State == TouchLocationState.Released)
                {
                    ScreenManager.getInstance().popScreen();
                    break;
                }
            }
#else
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
               ScreenManager.getInstance().popScreen();
            }
#endif
        }

        public override void Draw()
        {
            SpriteBatch.Begin();

            int w = Game.GraphicsDevice.Viewport.Width;
            int h = Game.GraphicsDevice.Viewport.Height;

            if (ScoreManager.getInstance().BestScore <= ScoreManager.getInstance().CurrentScore)
            {
                Vector2 textSize = fontscore.MeasureString("The new high score : " + ScoreManager.getInstance().CurrentScore.ToString());

                SpriteBatch.DrawString(fontscore, "The new high score : " + ScoreManager.getInstance().CurrentScore.ToString(),
                       new Vector2((w - textSize.X) / 2, (h / 2 - textSize.Y) / 2), Color.Red);
            }
            else
            {
                Vector2 textSize = fontscore.MeasureString("Score : " + ScoreManager.getInstance().CurrentScore.ToString());

                SpriteBatch.DrawString(fontscore, "Score : " + ScoreManager.getInstance().CurrentScore.ToString(),
                       new Vector2((w - textSize.X) / 2, (h / 2 - textSize.Y) / 2), Color.Red);
            }

            int tmp = (int)(ScoreManager.getInstance().CurrentScore / 100);

            String comment;

            if (tmp >= Comments.Count)
            {
                comment = Comments[Comments.Count - 1];
            }
            else
            {
                comment = Comments[tmp];
            }

            Vector2 commentSize = fonttext.MeasureString(comment);
            SpriteBatch.DrawString(fonttext, comment,
                       new Vector2((w - commentSize.X) / 2, (h/2) + (h / 2 - commentSize.Y) / 2), Color.White);
            //SpriteBatch.Draw(texture, position, Color.White);

            SpriteBatch.End();
        }
    }
}
