using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if WINDOWS_PHONE
    using Microsoft.Phone;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace WP7QuestGame.Components
{
   class FPSDisplay : AComponent
    {
        private int FrameCount = 0;
        private SpriteFont Font;
        private Stopwatch timer = Stopwatch.StartNew();
        private float fps = 0;

        public FPSDisplay(SpriteFont font) : base()
        {
            Font = font;
        }

        public override void Initialize()
        {
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.FrameCount++;
            spriteBatch.Begin();
#if WINDOWS_PHONE

            long mem = Microsoft.Phone.Info.DeviceStatus.ApplicationCurrentMemoryUsage / 1024;

            spriteBatch.DrawString(Font, fps.ToString() + " Fps\n" + mem + " kb", Vector2.Zero, Color.White);
#else
            spriteBatch.DrawString(Font, fps.ToString() + " Fps\n", Vector2.Zero, Color.White);
#endif
            spriteBatch.End();
     
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (timer.Elapsed > TimeSpan.FromSeconds(1))
            {
                fps = (float)(FrameCount / timer.Elapsed.TotalSeconds);
                timer.Stop();
                timer.Reset();
                timer.Start();
                FrameCount = 0;
            }
        }
    }
}
