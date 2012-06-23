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


namespace WP7QuestGame.Components
{
    public class Background : AComponent
    {

        private Texture2D background;
        private int Speed;
        private int offset;

        public bool AutoColorChange = false;
        private Color tint = new Color();

        private int r = 0, g = 0, b = 0;
        private bool goingUp = false;

        public Background(Texture2D tex) : base(true, -1)
        {
            Random rand = new Random();

            r = 55 + rand.Next(200);
            g = 0;
            b = 0;

            background = tex;
            offset = 0;
            Speed = GameSettings.BackGroundScrollSpeed;
        }

        public Background(Texture2D tex, int speed)
            : base(true, -1)
        {

            Random rand = new Random();

            r = 55 + rand.Next(200);
            g = 0;
            b = 0;

            background = tex;
            offset = 0;
            Speed = speed;
        }

        public Background(Texture2D tex, int speed, bool colorChange)
            : base(true, -1)
        {
            Random rand = new Random();

            r = 55 + rand.Next(200);
            g = 0;
            b = 0;

            background = tex;
            offset = 0;
            Speed = speed;
            AutoColorChange = colorChange;
        }

        public override void Initialize()
        {
            ;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(background, new Vector2(offset, 0), (AutoColorChange ? tint : Color.White));
            spriteBatch.Draw(background, new Vector2(offset + background.Width, 0), (AutoColorChange ? tint : Color.White));
            //spriteBatch.Draw(background, new Rectangle(offset, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height), Color.White);
            //spriteBatch.Draw(background, new Rectangle(offset + spriteBatch.GraphicsDevice.Viewport.Width, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height), Color.White);

        }

        private bool changeColor(ref int c, ref int next)
        {
            if (c == 255)
            {
                next++;
                if (next == 255)
                    c--;
                return true;
            }
            else if (c > 0 && next == 0)
            {
                c++;
                return true;
            }
            else if (c < 255 && next == 255)
            {
                c--;
                return true;
            }


            /*if (c != 0)
            {
                if (goingUp == false)
                {
                    c--;
                    if (c == 0)
                    {
                        next = 1;
                        goingUp = true;
                    }
                }
                else
                {
                    c++;
                    if (c >= 255)
                        goingUp = false;

                }
                    return true;
            }*/
                return false;
        }

        public override void Update(GameTime gameTime)
        {
            if (AutoColorChange)
            {
                changeColor(ref r, ref g);
                changeColor(ref g, ref b);
                changeColor(ref b, ref r);
                tint = new Color(r, g, b);
            }

            if (GameSettings.ScrollSpeed >= GameSettings.DefaultScrollSpeed)
            {
                int tmp = (Speed * gameTime.ElapsedGameTime.Milliseconds) / 1000;

                offset -= tmp;
                if (offset <= -background.Width)
                {
                    offset = 0;
                }
            }
        }
    }
}
