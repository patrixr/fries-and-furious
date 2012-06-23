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
    class AutoDestructiveBlock : Block
    {
        #region CONTRUCTION

        public AutoDestructiveBlock(Texture2D tex, Vector2 pos, Runner r)
            : base(tex, pos, r)
        {
        }

        public AutoDestructiveBlock(Texture2D tex, Vector2 pos, Runner r, int spriteFrameCount)
            : base(tex, pos, r, spriteFrameCount)
        {
        }

        public AutoDestructiveBlock(Texture2D tex, Vector2 pos, Runner r, int spriteFrameCount, Vector2 velocity)
            : base(tex, pos, r, spriteFrameCount, velocity)
        {
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            if (IsBeingDestroyed == false && Position.X < 3 * GameSettings.BlockWidth)
                Destroy();
            base.Update(gameTime);
        }


    }
}
