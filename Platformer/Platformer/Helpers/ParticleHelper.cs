using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Events;
using Platformer.Entities.Particles;
using Platformer.Interfaces;

namespace Platformer.Helpers
{
    class ParticleHelper : IEventListener
    {
        private List<Particle> particles;
        private AshSpawner ashSpawner;

        public ParticleHelper(List<Particle> particles)
        {
            this.particles = particles;

            ashSpawner = new AshSpawner(particles);

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
        }

		public void EventResponse(SimpleEvent simpleEvent)
		{
			particles.Clear();
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

            ashSpawner.Update(dt);
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
