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
    public class Block : AComponent
    {

        #region PROPERTIES

        protected Runner runner;

        public bool Blocking = true;

        /*
         * Velocity and position
         */
        protected Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            private set { _position = value; }
        }
        public Vector2 Velocity { get; set; }
        public Vector2 Dimensions { get; set; }

        /*
         * Sprite and animation
         */
        protected Texture2D _texture;
        protected int BlockAnimationDelay = GameSettings.BlockAnimationSwitchDelay;
        protected int CurrentFrame = 0;
        protected int SpriteFrameCount;
        protected int TimeElapsed = 0;


        protected bool IsBeingDestroyed;
        public bool Destroyed
        {
            get; private set;
        }

        #region COLLISION

        private Rectangle blockHitBoxTouch = new Rectangle();
        public Rectangle BlockHitBoxTouch
        {
            get
            {
                blockHitBoxTouch.X = (int)_position.X - 2 * GameSettings.BlockWidth;
                blockHitBoxTouch.Y = (int)_position.Y - 2 * GameSettings.BlockHeight;
                blockHitBoxTouch.Width = 5 * GameSettings.BlockWidth;
                blockHitBoxTouch.Height = 5 * GameSettings.BlockHeight;
                return blockHitBoxTouch;
            }
            private set
            {
                blockHitBoxTouch = value;
            }
        }

        private Rectangle blockHitBoxLeft = new Rectangle();
        public Rectangle BlockHitBoxLeft
        {
            get
            {
                blockHitBoxLeft.X = (int)_position.X;
                blockHitBoxLeft.Y = (int)_position.Y;
                blockHitBoxLeft.Width = GameSettings.BlockWidth / 3;
                blockHitBoxLeft.Height = GameSettings.BlockHeight;
                return blockHitBoxLeft;
            }
            private set
            {
                blockHitBoxLeft = value;
            }
        }

        private Rectangle blockHitBoxUp = new Rectangle();
        public Rectangle BlockHitBoxUp
        {
            get
            {
                blockHitBoxUp.X = (int)_position.X;
                blockHitBoxUp.Y = (int)_position.Y;
                blockHitBoxUp.Width = GameSettings.BlockWidth;
                blockHitBoxUp.Height = GameSettings.BlockHeight / 2;
                return blockHitBoxUp;
            }
            private set
            {
                blockHitBoxUp = value;
            }
        }

        private Rectangle blockHitBoxDown = new Rectangle();
        public Rectangle BlockHitBoxDown
        {
            get
            {
                blockHitBoxDown.X = (int)_position.X;
                blockHitBoxDown.Y = (int)_position.Y + GameSettings.BlockHeight/2;
                blockHitBoxDown.Width = GameSettings.BlockWidth;
                blockHitBoxDown.Height = GameSettings.BlockHeight / 2;
                return blockHitBoxDown;
            }
            private set
            {
                blockHitBoxDown = value;
            }
        }

        private Rectangle runnerHitBoxRight = new Rectangle();
        public Rectangle RunnerHitBoxRight
        {
            get
            {
                runnerHitBoxRight.X = (int)runner.Position.X + 2 * (GameSettings.RunnerWidth / 3);
                runnerHitBoxRight.Y = (int)runner.Position.Y + GameSettings.RunnerHeight / 3;
                runnerHitBoxRight.Width = GameSettings.RunnerWidth / 3;
                runnerHitBoxRight.Height = GameSettings.RunnerHeight/3;
                return runnerHitBoxRight;
            }
            private set
            {
                runnerHitBoxRight = value;
            }
        }

        private Rectangle runnerHitBoxDown = new Rectangle();
        public Rectangle RunnerHitBoxDown
        {
            get
            {
                runnerHitBoxDown.X = (int)(runner.Position.X) + GameSettings.RunnerWidth / 4;
                runnerHitBoxDown.Y = (int)(runner.Position.Y) + 2 * (GameSettings.RunnerHeight / 3);
                runnerHitBoxDown.Width = 2 * GameSettings.RunnerWidth / 4;
                runnerHitBoxDown.Height = GameSettings.RunnerHeight / 3;
                return runnerHitBoxDown;
            }
            private set
            {
                runnerHitBoxDown = value;
            }
        }

        private Rectangle runnerHitBoxUp = new Rectangle();
        public Rectangle RunnerHitBoxUp
        {
            get
            {
                runnerHitBoxUp.X = (int)(runner.Position.X) + GameSettings.RunnerWidth / 4;
                runnerHitBoxUp.Y = (int)(runner.Position.Y);
                runnerHitBoxUp.Width = 2 * GameSettings.RunnerWidth / 4;
                runnerHitBoxUp.Height = GameSettings.RunnerHeight / 3;
                return runnerHitBoxUp;
            }
            private set
            {
                runnerHitBoxUp = value;
            }
        }

        #endregion

        #endregion

        #region CONSTRUCTION
        public Block(Texture2D tex, Vector2 position, Runner r, int spriteFramesCount, Vector2 velocity)
            : base(true, 1)
        {
            _texture = tex;
            SpriteFrameCount = spriteFramesCount;
            IsBeingDestroyed = false;
            Destroyed = false;
            runner = r;
            Dimensions = new Vector2(GameSettings.BlockWidth, GameSettings.BlockHeight);
            Position = position;
            Velocity = velocity;
        }

        public Block(Texture2D tex, Vector2 position, Runner r, int spriteFramesCount)
            : base(true, 4)
        {
            _texture = tex;
            runner = r;
            IsBeingDestroyed = false;
            Destroyed = false;
            SpriteFrameCount = spriteFramesCount;
            Position = position;
            Velocity = new Vector2(-((float)GameSettings.ScrollSpeed), 0f);
            Dimensions = new Vector2(GameSettings.BlockWidth, GameSettings.BlockHeight);
        }

        public Block(Texture2D tex, Vector2 position, Runner r)
            : base(true, 4)
        {
            _texture = tex;
            runner = r;
            IsBeingDestroyed = false;
            Destroyed = false;
            SpriteFrameCount = 1;
            Position = position;
            Velocity = new Vector2(-((float)GameSettings.ScrollSpeed), 0f);
            Dimensions = new Vector2(GameSettings.BlockWidth, GameSettings.BlockHeight);
        }

        #endregion

        public override void Initialize()
        {
            ;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsBeingDestroyed)
                spriteBatch.Draw(_texture, Position,
                                        new Rectangle(CurrentFrame * GameSettings.BlockWidth, GameSettings.BlockHeight, GameSettings.BlockWidth, GameSettings.BlockHeight),
                                        Color.White);
            else
                spriteBatch.Draw(_texture, Position,
                                        new Rectangle(CurrentFrame * GameSettings.BlockWidth, 0, GameSettings.BlockWidth, GameSettings.BlockHeight),
                                        Color.White);
            //spriteBatch.Draw(_texture, BlockHitBox, Color.Blue);
            //spriteBatch.Draw(_texture, RunnerHitBox, Color.Red);

        }

        public override void Update(GameTime gameTime)
        {
            TimeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeElapsed > (IsBeingDestroyed ? GameSettings.BlockDestructionSwitchDelay : GameSettings.BlockAnimationSwitchDelay))
            {
                TimeElapsed = 0;
                CurrentFrame++;
                if (CurrentFrame >= SpriteFrameCount)
                {
                    if (IsBeingDestroyed)
                        Destroyed = true;
                    else
                        CurrentFrame = 0;
                }
            }

            _position.X += (-GameSettings.ScrollSpeed* gameTime.ElapsedGameTime.Milliseconds) / 1000;

           if (RunnerHitBoxRight.Intersects(BlockHitBoxLeft))
            {
                Vector2 pos = runner.Position;
                pos.X +=(Velocity.X * gameTime.ElapsedGameTime.Milliseconds) / 1000;
                runner.Position = pos;
            }

            _position.Y += (Velocity.Y * gameTime.ElapsedGameTime.Milliseconds) / 1000;
        }

        protected virtual void Destroy()
        {
            if (IsBeingDestroyed == false)
            {
                IsBeingDestroyed = true;
                CurrentFrame = 0;
            }
        }

        public virtual void CheckUserTouch(TouchLocation tl)
        {
           
        }
 
    }
}
