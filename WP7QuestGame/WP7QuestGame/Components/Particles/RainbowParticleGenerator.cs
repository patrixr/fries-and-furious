using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WP7QuestGame.Components.Particles
{
    class RainbowParticleGenerator : IParticleGenerator
    {
        private float r, g, b;
        private Texture2D _texture;
        private Random _random = new Random();

        public RainbowParticleGenerator(Texture2D tex)
        {
            _texture = tex;
            r = 255;
            g = 0;
            b = 0;
        }

        public Particle GenerateParticle()
        {
            throw new NotImplementedException();
        }

        public Particle GenerateParticle(Vector2 position)
        {
            Vector2 velocity = new Vector2(-4 * (float)(_random.NextDouble()) - 5, 0);
            //Vector2 velocity = new Vector2(-10, 0);
            float y = position.Y + ((_random.Next() % 2 == 0) ? 1 : -1) * (_random.Next(GameSettings.RunnerHeight / 4));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);
            Color color;

            color = new Color((float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble());


            int lifespan = _random.Next(40);

            return new Particle(_texture, new Vector2(position.X + GameSettings.RunnerWidth / 3 , y + GameSettings.RunnerHeight / 2),
                                velocity, angle, angularVelocity, color, 0.5f, lifespan);
            
            //float y = position.Y + ((_random.Next() % 2 == 0) ? 1 : -1) * (_random.Next(GameSettings.RunnerHeight/2));


            //return new Particle(_texture, new Vector2(position.X + GameSettings.RunnerWidth / 2, y + GameSettings.RunnerHeight / 2), new Vector2(-100, 0), 0, 10, new Color(r, g, b), 0.2f, 10);
        }
    }
}
