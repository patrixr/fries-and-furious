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
    public class Particle : AComponent
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }     
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; } 
        public Color Color { get; set; } 
        public float Size { get; set; }
        public int Lifespan { get; set; }

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int lifespan) : base()
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            Lifespan = lifespan;
        }

        public override void Initialize()
        {
            return;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color, Angle, origin, Size, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            Lifespan--;
            Position += Velocity;
            //this.Velocity = new Vector2(this.Velocity.X - 0.1f, this.Velocity.Y - 0.1f);
            Angle += AngularVelocity;
        }
    }
}
