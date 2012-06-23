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
    public class RandomParticleGenerator : IParticleGenerator
    {
        private Random random = new Random();
        public List<Texture2D> Textures { get; set; }
        public bool RandomTint { get; set; }
        public Color TintColor { get; set; }
        public float ScaleMax { get; set; }
        public float ScaleMin { get; set; }
        public int Lifespan { get; set; }
        

        public RandomParticleGenerator(bool randomTint, float scaleMin, float scaleMax, int lifespan)
        {
            Textures = new List<Texture2D>();
            Color tintColor = Color.CornflowerBlue;
            RandomTint = randomTint;
            TintColor = tintColor;
            ScaleMin = scaleMin;
            ScaleMax = scaleMax;
            Lifespan = lifespan;
        }

        public RandomParticleGenerator()
        {
            Textures = new List<Texture2D>();
            Color tintColor = Color.CornflowerBlue;
            RandomTint = false;
            TintColor = tintColor;
            ScaleMin = 0.0f;
            ScaleMax = 1f;
            Lifespan = 20;
        }

        private byte AddToColorComponent(byte cmp, byte toAdd)
        {
            int test = (int)(cmp) + (int)(toAdd);
            if (test > 255 || test < 0)
                return cmp;
            else
                return (byte)test;
        }

        public Particle GenerateParticle()
        {
            Texture2D texture = Textures[random.Next(Textures.Count)];
            Vector2 position = Vector2.Zero;
            Vector2 velocity = new Vector2(5f * (float)(random.NextDouble() * 2 - 1), 5f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color;
            if (RandomTint)
            {
                color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            }
            else
            {
                byte tmp = (byte)((random.Next() % 2 == 0 ? 1 : -1) * (byte)random.Next(30));
                color = TintColor;// new Color(AddToColorComponent(TintColor.R, tmp), AddToColorComponent(TintColor.G, tmp),
                              // AddToColorComponent(TintColor.B, tmp), TintColor.A);
            }
            float size = ScaleMin + (float)random.NextDouble();
            if (size > ScaleMax)
                size = ScaleMax;
            int lifespan = Lifespan + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifespan);
        }

        public Particle GenerateParticle(Vector2 position)
        {
            Texture2D texture = Textures[random.Next(Textures.Count)];
            Vector2 velocity = new Vector2(1f * (float)(random.NextDouble() * 2 - 1), 1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color;
            if (RandomTint)
            {
                color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            }
            else
            {
                byte tmp = (byte)((random.Next() % 2 == 0 ? 1 : -1) * (byte)random.Next(30));
                color = new Color(AddToColorComponent(TintColor.R, tmp), AddToColorComponent(TintColor.G, tmp),
                               AddToColorComponent(TintColor.B, tmp), TintColor.A);
            }
            float size = ScaleMin + (float)random.NextDouble();
            if (size > ScaleMax)
                size = ScaleMax;
            int lifespan = Lifespan + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifespan);
        }
    }
}
