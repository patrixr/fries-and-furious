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

namespace WP7QuestGame.Components
{
    public class Runner : AComponent
    {
        private int _stepCount = 8;
        private int _currentStep = 0;
        private Texture2D _texture;
        private int _elapsed = 0;
        public bool IsAlive = true;
        public int Direction { get; private set; }
        private bool _isOnGround = false;
        //private int XSpeed = 0;
        private List<Block> _map;

        //private Map _map;

        private Rectangle touchHitBox = new Rectangle();
        public Rectangle TouchHitBox
        {
            get
            {
                touchHitBox.X = (int)Position.X - 2 * GameSettings.RunnerWidth;
                touchHitBox.Y = (int)Position.Y - 2 * GameSettings.RunnerHeight;
                touchHitBox.Width = 5 * GameSettings.RunnerWidth;
                touchHitBox.Height = 5 * GameSettings.RunnerHeight;
                return touchHitBox;

            }
            private set
            {
                touchHitBox = value;
            }
        }


        private Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Runner(Texture2D spriteSheet, Vector2 pos, List<Block> m)
            : base()
        {
            Direction = 1;
            _texture = spriteSheet;
            Position = pos;
            _map = m;
        }

        public void SwitchGravity()
        {
            if (_isOnGround)
            {
                _isOnGround = false;
                Direction *= -1;
            }
        }

        public override void Initialize()
        {
            ;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int y = (_isOnGround ? 0 : GameSettings.RunnerHeight);

          
            if (Direction == 1)
                spriteBatch.Draw(_texture, Position, new Rectangle(_currentStep * GameSettings.RunnerWidth, y, GameSettings.RunnerWidth, GameSettings.RunnerHeight), Color.White);
            else
            {
                //if (_isOnGround)
                    spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, GameSettings.RunnerWidth, GameSettings.RunnerHeight),
                        new Rectangle(_currentStep * GameSettings.RunnerWidth, y, GameSettings.RunnerWidth, GameSettings.RunnerHeight), Color.White, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                //else
                  //  spriteBatch.Draw(_texture, Position, new Rectangle(_currentStep * 58, y, 50, 50), Color.White);
            }
            //spriteBatch.Draw(_texture, new Rectangle((int)(_position.X), (int)(_position.Y + 2*(_texture.Height/3)), (int)_texture.Width/8, _texture.Height/3), Color.Red);
           
        }

        #region COLLISION //@TODO OPTIMIZE

        private bool CollisionUp()
        {
            Rectangle runRect = new Rectangle((int)(_position.X) + GameSettings.BlockWidth/4,
                                             (int)(_position.Y),
                                             2 * GameSettings.RunnerWidth/4, GameSettings.RunnerHeight / 3);
            Rectangle blockRect = new Rectangle();

            foreach (Block b in _map)
            {
                if (b.Blocking == false)
                    continue;
                blockRect.X = (int)b.Position.X;
                blockRect.Y = (int)b.Position.Y + 2 * (GameSettings.BlockHeight/3);
                blockRect.Width = GameSettings.BlockWidth;
                blockRect.Height = GameSettings.BlockHeight / 3;

                if (runRect.Intersects(blockRect))
                {
                    _isOnGround = true;
                    return true;
                }
            }
            _isOnGround = false;
            return false;
        }

        private bool CollisionDown()
        {
            Rectangle runRect = new Rectangle((int)(_position.X) + GameSettings.RunnerWidth/4,
                                              (int)(_position.Y + 2 * (GameSettings.RunnerHeight / 3)),
                                              2* GameSettings.RunnerWidth/4, GameSettings.RunnerHeight / 3);
            Rectangle blockRect = new Rectangle();

            foreach (Block b in _map)
            {
                if (b.Blocking == false)
                    continue;
                blockRect.X = (int)b.Position.X;
                blockRect.Y = (int)b.Position.Y;
                blockRect.Width = GameSettings.BlockWidth;
                blockRect.Height = GameSettings.BlockHeight / 3;

                if (runRect.Intersects(blockRect))
                {
                    _isOnGround = true;
                    return true;
                }
            }
            _isOnGround = false;
            return false;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            _elapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (_elapsed >= GameSettings.RunnerAnimationSwitchDelay)
            {
                _elapsed = 0;
                _currentStep++;
                if (_currentStep == _stepCount)
                    _currentStep = 0;
            }

            if (Direction > 0)
            {
                if (CollisionDown() == false)
                    _position.Y += (Direction * GameSettings.RunnerYSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000;
            }
            else
            {
                if (CollisionUp() == false)
                    _position.Y += (Direction * GameSettings.RunnerYSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000;
            }
            /*_position.X += (XSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000;
            if (this._isOnGround && Position.X < 350)
                XSpeed = 50;
            else if (this._isOnGround && Position.X > 450)
                XSpeed = -50;
            else
                XSpeed = 0;*/
        }
    }
}
