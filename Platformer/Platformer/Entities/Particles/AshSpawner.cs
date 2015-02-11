using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Entities.Particles
{
	class AshSpawner
	{
		private const int MAX_ASH_PARTICLES = 25; 

		private List<Particle> particles;

		public AshSpawner(List<Particle> particles)
		{
			this.particles = particles;
		}

		public void Update()
		{
		}
	}
}
