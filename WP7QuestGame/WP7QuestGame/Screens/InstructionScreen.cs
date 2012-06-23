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
    class InstructionScreen : Screen
    {

        private int currentPic = 0;

        public InstructionScreen(GameLoop game)
            : base(game)
        {
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
                    if (currentPic == 1)
                    {
                        ScreenManager.getInstance().popScreen();
                        break;
                    }
                    else
                    {
                        currentPic++;
                    }
                }
            }
#else
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                if (currentPic == 1)
                {
                    ScreenManager.getInstance().popScreen();
                }
                else
                {
                    currentPic++;
                }
            }
#endif
        }

        public override void Draw()
        {
            SpriteBatch.Begin();

            if (currentPic == 0)
                SpriteBatch.Draw(Content.Load<Texture2D>("intructions1"), Vector2.Zero, Color.White); 
            else
                SpriteBatch.Draw(Content.Load<Texture2D>("intructions2"), Vector2.Zero, Color.White); 

            SpriteBatch.End();
        }
    }
}
