using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
#if WINDOWS_PHONE

using Microsoft.Phone.Controls;

#endif
using Microsoft.Xna.Framework.Input.Touch;

//#endif
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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SurvivalModeScreen : Screen
    {

        #region PROPERTIES
        public List<Block> Blocks { get; set; }

        private List<Block> _toDelete;
        private Runner _runner;
        private ParticleEmitter _particleEmitter;
        private ChunkGenerator _blockGenerator;
        //private LolBlockGenerator _blockGenerator;

        private Sprite _pausebutton;

#if WINDOWS
        private Sprite _exitbutton;
#endif


        private Vector2 ScorePosition = new Vector2(0, 0);
        private Rectangle _buttonHitBox;

        private bool IsAlive = true;
        private int Elapsed = 0;

        private bool _blocksInit = false;

        private Texture2D _blockTexture;
        private Texture2D _runnerTexture;
        private Random _random = new Random();

//#if WINDOWS_PHONE
        private TouchCollection touches;
//#endif

        private int _blockMaxH;
        private int _blockMaxW;
        private int _mapHeight, _mapWidth;

#if DEBUG
        bool _spacePressed = false;
#endif

        #endregion

        #region CONTRUCTION 

        public SurvivalModeScreen(GameLoop game)
            : base(game)
        {
            _mapHeight = game.GraphicsDevice.Viewport.Height;
            _mapWidth = game.GraphicsDevice.Viewport.Width;
            _blockTexture = Content.Load<Texture2D>(GameSettings.BlockName);
            _runnerTexture = Content.Load<Texture2D>(GameSettings.RunnerName);
            _blockMaxH = _mapHeight / GameSettings.BlockHeight;
            _blockMaxW = _mapWidth / GameSettings.BlockWidth;
        }

        public override void Initialize()
        {   
            Blocks = new List<Block>();
            _toDelete = new List<Block>();

            /*bool toto = File.Exists("Content/Maps/0.txt");
            bool tata = File.Exists("../0.txt");
            StreamReader sr = new StreamReader("Content/Maps/0.txt");

            String s = sr.ReadLine();
            Console.WriteLine(s);
             */

            if (GameSettings.AllowMusic && GameSettings.VolumeOn)
            {
                MediaPlayer.Play(Content.Load<Song>("gamesound"));
                MediaPlayer.IsRepeating = true;
            }

            _buttonHitBox = new Rectangle(0, _mapHeight - 120, 120, 120);
            ScoreManager.getInstance().CurrentScore = 0;
           
            _runner = new Runner(_runnerTexture, new Vector2(_mapWidth / 2, _mapHeight / 2), Blocks);
            _runner.ZIndex = 5;
            //_blockGenerator = new LolBlockGenerator(_runner, _mapWidth, _mapHeight, Content);
            _blockGenerator = new ChunkGenerator(_runner, _mapWidth, _mapHeight, Content);

            _particleEmitter = new ParticleEmitter(new RainbowParticleGenerator(Content.Load<Texture2D>("star")), new Vector2(700, 240), GameSettings.ParticleGenerationCount);
            _particleEmitter.ZIndex = 2;
            _particleEmitter.Stop();

#if WINDOWS
            _exitbutton = new Sprite(Content.Load<Texture2D>("ingameexitbutton"), new Vector2(0, 50));
#endif

            _pausebutton = new Sprite(Content.Load<Texture2D>("pause"), new Vector2(_mapWidth - 75, 10));

            this.Components.Add(new Background(Content.Load<Texture2D>("background2"), GameSettings.BackGroundScrollSpeed / 2, true));
            this.Components.Add(new Background(Content.Load<Texture2D>("background1"), GameSettings.BackGroundScrollSpeed, true));
            
            this.Components.Add(_particleEmitter);
            this.Components.Add(_runner);
            base.Initialize();
        }

        #endregion

        private void InitBlocks()
        {
            for (int i = 0; i < _blockMaxW; ++i)
            {
                Block tmp1 = new Block(_blockTexture, new Vector2(i * GameSettings.BlockWidth, 0), _runner);
                Block tmp2 = new Block(_blockTexture, new Vector2(i * GameSettings.BlockWidth, _mapHeight - GameSettings.BlockHeight), _runner);
                Blocks.Add(tmp1);
                Blocks.Add(tmp2);
                //this.Components.Add(tmp1);
                //this.Components.Add(tmp2);
            }
            _blocksInit = true;
        }

        public override void UnloadContent()
        {
            if (GameSettings.AllowMusic && GameSettings.VolumeOn)
                MediaPlayer.Stop();
        }

        public override void HandleInput()
        {
            bool switched = false;

//#if WINDOWS_PHONE
            touches = TouchPanel.GetState();

            foreach (TouchLocation tl in touches)
            {
                if (tl.State == TouchLocationState.Released)
                {
                    if (tl.Position.Y > 0 && tl.Position.Y < 80 && tl.Position.X > _mapWidth - 100)
                    {
                        ScreenManager.getInstance().pushScreen(new PauseScreen(Game));
                    }
               
                }
                if (tl.State == TouchLocationState.Pressed)
                {

                    if (switched == false && _buttonHitBox.Intersects(new Rectangle((int)tl.Position.X, (int)tl.Position.Y, 1, 1)))
                    {
                        switched = true;
                        _runner.SwitchGravity();
                    }
                }
            }

//#endif

/*#if WINDOWS

        if (Mouse.GetState().LeftButton == ButtonState.Released)
        {
            if (Mouse.GetState().X > 50 && Mouse.GetState().Y < 125 && Mouse.GetState().X < 50)
            {
                MediaPlayer.Stop();
                ScreenManager.getInstance().popScreen();
            }
        }
        else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            if (switched == false && _buttonHitBox.Intersects(new Rectangle((int)Mouse.GetState().X, (int)Mouse.GetState().X, 1, 1)))
            {
                switched = true;
                _runner.SwitchGravity();
            }
        }

#endif*/

#if DEBUG

            if (_spacePressed == false && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _runner.SwitchGravity();
                _spacePressed = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space))
                _spacePressed = false;
#endif



            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {

            if (IsAlive)
            {
                Elapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (Elapsed >= 1000)
                {
                    ScoreManager.getInstance().CurrentScore += 1 + ((GameSettings.ScrollSpeed * 10) / GameSettings.MaxScrollSpeed);
                    Elapsed -= 1000;
                    if (ScoreManager.getInstance().CurrentScore > ScoreManager.getInstance().BestScore)
                        _particleEmitter.Start();
                }
            }

            /**
             * Generate Blocks
             */
            if (_blocksInit == false)
            {
                InitBlocks();
            }

            List<Block> newBlocks = _blockGenerator.GenerateChunk();
            if (newBlocks != null)
            {
                foreach (Block b in newBlocks)
                {
                    Blocks.Add(b);
                    //Components.Add(b);
                }
            }

            /*
             * Check for end of game
             */

            if (_runner.Position.X <= -GameSettings.RunnerWidth || _runner.Position.Y <= -GameSettings.RunnerHeight || _runner.Position.Y >= _mapHeight)
            {
                _runner.IsAlive = false;
                if (GameSettings.ScrollSpeed > 0)
                    GameSettings.ScrollSpeed -= 10;
                else
                {
                    if (GameSettings.AllowMusic && GameSettings.VolumeOn)
                        MediaPlayer.Stop();
                    GameSettings.ScrollSpeed = GameSettings.DefaultScrollSpeed;
                    ScreenManager.getInstance().popScreen();
                    if (ScoreManager.getInstance().CurrentScore > ScoreManager.getInstance().BestScore)
                    {
                        ScoreManager.getInstance().SaveScore();
                        ScoreManager.getInstance().CurrentScore = 0;
                    }
                    return;
                }
            }

            _particleEmitter.Position = _runner.Position;

            /*
             * Delete useless blocks
             */
            _toDelete.Clear();
            foreach (Block b in Blocks)
            {
                if (b.Position.X < -_blockTexture.Width || b.Destroyed)
                    _toDelete.Add(b);
                else
                {
                    b.Update(gameTime);
                    foreach (TouchLocation tl in touches)
                    {
                        b.CheckUserTouch(tl);
                    }

                }
            }

            /*
             * Cleanup
             */
            foreach (Block b in _toDelete)
            {
                Blocks.Remove(b);
                //Components.Remove(b);
            }

            /*
             * Update
             */
            base.Update(gameTime);
        }

        public override void Draw()
        {
            SpriteBatch.Begin();
            for (int i = 0; i < Components.Count; ++i)
                Components[i].Draw(SpriteBatch);
            //Score
            SpriteFont font = Content.Load<SpriteFont>("ScoreFont");

            ScorePosition.X = _mapWidth - font.MeasureString("Score : " + ScoreManager.getInstance().CurrentScore.ToString()).X;
            ScorePosition.Y = 55;
            if (ScoreManager.getInstance().CurrentScore > ScoreManager.getInstance().BestScore)
                SpriteBatch.DrawString(font, "Score : " + ScoreManager.getInstance().CurrentScore.ToString(), ScorePosition, Color.LightSalmon);
            else
                SpriteBatch.DrawString(font, "Score : " + ScoreManager.getInstance().CurrentScore.ToString(), ScorePosition, Color.Gray);

            foreach (Block b in Blocks)
                b.Draw(SpriteBatch);

            // Button
            SpriteBatch.Draw(Content.Load<Texture2D>("gswitch"), new Vector2(0, _mapHeight - 120), Color.White);
            
#if WINDOWS
            _exitbutton.Draw(SpriteBatch);
#endif
            _pausebutton.Draw(SpriteBatch);

            SpriteBatch.End();
        }

    }
}
