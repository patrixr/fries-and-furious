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
    class PauseScreen : Screen
    {

        private Vector2 position;
        private Texture2D texture;

        public PauseScreen(GameLoop game)
            : base(game)
        {
            this.BlocksDraw = false;
            texture = Content.Load<Texture2D>("pausedialog");
            position = new Vector2((game.GraphicsDevice.Viewport.Width - texture.Width) / 2, (game.GraphicsDevice.Viewport.Height - texture.Height) / 2);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

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

            SpriteBatch.Draw(texture, position, Color.White);

            SpriteBatch.End();
        }
    }
}
