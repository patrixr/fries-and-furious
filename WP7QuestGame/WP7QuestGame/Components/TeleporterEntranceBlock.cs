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
    class TeleporterEntranceBlock : Block
    {

        public TeleporterEntranceBlock Exit { get; set; }
        private bool activated = true;

#region COLLISION

        private Rectangle teleporterHitBox = new Rectangle();
        private Rectangle TeleporterHitBox
        {
            get
            {
                teleporterHitBox.X = (int)Position.X + GameSettings.BlockWidth/3;
                teleporterHitBox.Y = (int)Position.Y + GameSettings.BlockHeight/3;
                teleporterHitBox.Width = GameSettings.BlockWidth/3;
                teleporterHitBox.Height = GameSettings.BlockHeight/3;
                return teleporterHitBox;
            }
            set
            {
                teleporterHitBox = value;
            }
        }

        private Rectangle runnerCenterHitBox = new Rectangle();
        private Rectangle RunnerCenterHitBox
        {
            get
            {
                runnerCenterHitBox.X = (int)runner.Position.X + GameSettings.RunnerWidth/3;
                runnerCenterHitBox.Y = (int)runner.Position.Y + GameSettings.RunnerHeight/3;
                runnerCenterHitBox.Width = GameSettings.RunnerWidth/3;
                runnerCenterHitBox.Height = GameSettings.RunnerHeight/3;
                return runnerCenterHitBox;
            }
            set
            {
                runnerCenterHitBox = value;
            }
        }

#endregion

        #region CONTRUCTION

        public TeleporterEntranceBlock(Texture2D tex, Vector2 pos, Runner r)
            : base(tex, pos, r)
        {
            Exit = null;
            Blocking = false;
        }

        public TeleporterEntranceBlock(Texture2D tex, Vector2 pos, Runner r, int spriteFrameCount)
            : base(tex, pos, r, spriteFrameCount)
        {
            Exit = null;
            Blocking = false;
        }

        public TeleporterEntranceBlock(Texture2D tex, Vector2 pos, Runner r, int spriteFrameCount, Vector2 velocity)
            : base(tex, pos, r, spriteFrameCount, velocity)
        {
            Exit = null;
            Blocking = false;
        }

        #endregion

        public void Deactivate()
        {
            activated = false;
        }

        public override void Update(GameTime gameTime)
        {
        
            if (runner.IsAlive && activated && Exit != null && TeleporterHitBox.Intersects(RunnerCenterHitBox))
            {
               Exit.Deactivate();
               runner.Position = new Vector2(Exit.Position.X, Exit.Position.Y - 2* (GameSettings.RunnerHeight - GameSettings.BlockHeight));
            }

            TimeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeElapsed > (IsBeingDestroyed ? GameSettings.BlockDestructionSwitchDelay : GameSettings.BlockAnimationSwitchDelay))
            {
                TimeElapsed = 0;
                CurrentFrame++;
                if (CurrentFrame >= SpriteFrameCount)
                {
                  CurrentFrame = 0;
                }
            }

            _position.X += (-GameSettings.ScrollSpeed* gameTime.ElapsedGameTime.Milliseconds) / 1000;
            _position.Y += (Velocity.Y * gameTime.ElapsedGameTime.Milliseconds) / 1000;

        }
    }
}
