using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Particles;

namespace Platformer.Helpers
{
    class ParticleHelper
    {
        private List<Particle> particles;
        private AshSpawner ashSpawner;

        public ParticleHelper()
        {
            particles = new List<Particle>();
            ashSpawner = new AshSpawner(particles);
        }

        public void Update(float dt)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                Particle particle = particles[i];

                if (particle.Destroyed)
                {
                    particles.RemoveAt(i);
                }
                else
                {
                    particle.Update(dt);
                }
            }

            ashSpawner.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(sb);
            }
        }
    }
}
