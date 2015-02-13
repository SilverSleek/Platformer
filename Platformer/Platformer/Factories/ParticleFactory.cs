using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Platformer.Entities.Particles;

namespace Platformer.Factories
{
    class ParticleFactory
    {
        private static List<Particle> particles;

        public static void Initialize(List<Particle> particles)
        {
            ParticleFactory.particles = particles;
        }

        public static void CreateParticle(ParticleTypes particleType, Vector2 position, Vector2 velocity)
        {
            Particle particle = null;

            switch (particleType)
            {
                case ParticleTypes.ASH:
                    particle = new Ash(position, velocity);
                    break;

                case ParticleTypes.EMBER:
                    particle = new Ember(position, velocity);
                    break;
            }

            particles.Add(particle);
        }
    }
}
