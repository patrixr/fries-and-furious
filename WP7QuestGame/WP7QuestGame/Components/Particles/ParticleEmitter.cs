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

namespace WP7QuestGame.Components.Particles
{
    public class ParticleEmitter : AComponent
    {
        #region Public Properties

        public int GenerationCount { get; set; }
        public Vector2 Position { get; set; }

        #endregion       

        #region Private Properties

        private List<Particle> Particles;
        IParticleGenerator Generator;
        bool IsRunning = true;

        #endregion

        #region Constructors

        public ParticleEmitter(IParticleGenerator gen, Vector2 location) : base()
        {
            Position = location;
            GenerationCount = 10;
            Particles = new List<Particle>();
            Generator = gen;
        }

        public ParticleEmitter(IParticleGenerator gen, Vector2 location, int generationCount)
            : base()
        {
            Position = location;
            GenerationCount = generationCount;
            Particles = new List<Particle>();
            Generator = gen;
        }

        #endregion

        #region AComponent implementation

        public override void Initialize()
        {
            return;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in Particles)
            {
                p.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsRunning)
            {
                for (int i = 0; i < GenerationCount; i++)
                {
                    Particles.Add(Generator.GenerateParticle(Position));
                }
            }
            for (int particle = 0; particle < Particles.Count; particle++)
            {
                Particles[particle].Update(gameTime);
                if (Particles[particle].Lifespan <= 0)
                {
                    Particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        #endregion

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
