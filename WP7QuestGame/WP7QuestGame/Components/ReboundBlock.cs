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
    class ReboundBlock : Block
    {
        
        #region CONTRUCTION

        public ReboundBlock(Texture2D tex, Vector2 pos, Runner r)
            : base(tex, pos, r)
        {
        }

        public ReboundBlock(Texture2D tex, Vector2 pos, Runner r, int spriteFrameCount)
            : base(tex, pos, r, spriteFrameCount)
        {
        }

        public ReboundBlock(Texture2D tex, Vector2 pos, Runner r, int spriteFrameCount, Vector2 velocity)
            : base(tex, pos, r, spriteFrameCount, velocity)
        {
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            if (RunnerHitBoxDown.Intersects(BlockHitBoxUp))
            {
                if (runner.Direction == 1)
                    runner.SwitchGravity();
            }
            else if (RunnerHitBoxUp.Intersects(BlockHitBoxDown))
            {
                if (runner.Direction == -1)
                    runner.SwitchGravity();
            }

            base.Update(gameTime);
        }


    }
}
