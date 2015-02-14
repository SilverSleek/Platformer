using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities.Particles
{
	class AshSpawner
	{
		private const int AVERAGE_ASH_PARTICLES_PER_SECOND = 2;
		private const int OFFSCREEN_HEIGHT = 200;
		private const int GENERATION_OFFSET = 10;
		private const int MIN_SPEED_X = -20;
		private const int MAX_SPEED_X = 20;
		private const int MIN_SPEED_Y = 25;
		private const int MAX_SPEED_Y = 50;

		private List<Particle> particles;
		private Random random;
			
		public AshSpawner(List<Particle> particles)
		{
			this.particles = particles;

			random = new Random();
		}

		public void Update(float dt)
		{
            if ((float)random.NextDouble() <= AVERAGE_ASH_PARTICLES_PER_SECOND * dt)
			{
				Rectangle visibleArea = Camera.Instance.VisibleArea;
				Rectangle spawnRect = new Rectangle(visibleArea.X, visibleArea.Y - OFFSCREEN_HEIGHT, Constants.SCREEN_WIDTH, OFFSCREEN_HEIGHT);

				float x = Functions.GetRandomValue(spawnRect.Left + GENERATION_OFFSET, spawnRect.Right - GENERATION_OFFSET);
				float y = Functions.GetRandomValue(spawnRect.Top + GENERATION_OFFSET, spawnRect.Bottom - GENERATION_OFFSET);
				float velocityX = Functions.GetRandomValue(MIN_SPEED_X, MAX_SPEED_X);
				float velocityY = Functions.GetRandomValue(MIN_SPEED_Y, MAX_SPEED_Y);

				particles.Add(new Ash(new Vector2(x, y), new Vector2(velocityX, velocityY)));
			}
		}
	}
}
