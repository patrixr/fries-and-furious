using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WP7QuestGame.Components.Particles
{
    public interface IParticleGenerator
    {
        Particle GenerateParticle();
        Particle GenerateParticle(Vector2 position);
    }
}
