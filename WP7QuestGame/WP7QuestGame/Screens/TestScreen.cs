using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WP7QuestGame.Components;
using WP7QuestGame.Components.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WP7QuestGame.Screens
{
    class TestScreen : Screen
    {
        ParticleEmitter particleEmitter;
        RandomParticleGenerator particleGen;
        bool clicked = false;
        bool emit = false;
        bool inputChanged = false;

        public bool randomColor
        {
            get;
            set;
        }

        public TestScreen(GameLoop game) : base(game)
        {
        }

        public override void Initialize()
        {
            particleGen = new RandomParticleGenerator();
            particleGen.Textures.Add(Content.Load<Texture2D>("circle"));
            //particleGen.Textures.Add(Content.Load<Texture2D>("star"));
            //particleGen.Textures.Add(Content.Load<Texture2D>("diamond"));
            particleGen.RandomTint = false;
            particleGen.Lifespan = 10;
            particleGen.ScaleMin = 0.2f;
            particleGen.ScaleMax = 1f;
            particleGen.TintColor = Color.White;

            particleEmitter = new ParticleEmitter(particleGen, new Vector2(300, 300));
            particleEmitter.GenerationCount = 3;

            //Sprite tmp = new Sprite(Content.Load<Texture2D>("blob"), new Vector2(300, 300));

            //tmp.ZIndex = 2;
            particleEmitter.ZIndex = 1;

            //AddComponent(tmp);
            AddComponent(particleEmitter);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (emit && inputChanged)
            {
                particleEmitter.Start();
            }
            else if (inputChanged)
            {
                particleEmitter.Stop();
            }
            particleEmitter.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            base.Update(gameTime);
            //emit = false;
            //particleEmitter.Stop();
        }

        public override void Draw()
        {
            
            //effect.
            
            
            base.Draw();
        }

        public override void HandleInput()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && clicked == false)
            {
                emit = true;
                clicked = true;
                inputChanged = true;
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                emit = false;
                clicked = false;
                inputChanged = true;
            }
            base.HandleInput();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

    }
}
