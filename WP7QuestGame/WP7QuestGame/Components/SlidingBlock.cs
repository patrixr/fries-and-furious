using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace WP7QuestGame.Components
{
    class SlidingBlock : Block
    {

        public int Direction = 0;
        private bool moved = false;
        private bool touched = false;
        private int distanceMoved = 0;
        private Vector2 savedCoord = Vector2.Zero;

        #region CONTRUCTION

        public SlidingBlock(Texture2D tex, Vector2 pos, Runner r)
            : base(tex, pos, r)
        {
            if (pos.Y > 240)
                Direction = -1;
            else
                Direction = 1;
        }

        public SlidingBlock(Texture2D tex, Vector2 pos, Runner r, int spriteFrameCount)
            : base(tex, pos, r, spriteFrameCount)
        {
            if (pos.Y > 240)
                Direction = -1;
            else
                Direction = 1;
        }

        public SlidingBlock(Texture2D tex, Vector2 pos, Runner r, int spriteFrameCount, Vector2 velocity)
            : base(tex, pos, r, spriteFrameCount, velocity)
        {
            if (pos.Y > 240)
                Direction = -1;
            else
                Direction = 1;
        }

        #endregion

        public override void CheckUserTouch(TouchLocation tl)
        {
            if (moved)
                return;

            if (BlockHitBoxTouch.Intersects(new Rectangle((int)tl.Position.X, (int)tl.Position.Y, 1, 1)))
            {
                moved = true;
            }
            
            base.CheckUserTouch(tl);
        }

        public override void Update(GameTime gameTime)
        {
            if (moved && distanceMoved < 3 * GameSettings.BlockHeight)
            {
                _position.Y += (Direction >= 0 ? 1 : -1) * (GameSettings.ScrollSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000;
                distanceMoved += (GameSettings.ScrollSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000;
            }

            base.Update(gameTime);
        }
        

    }
}
