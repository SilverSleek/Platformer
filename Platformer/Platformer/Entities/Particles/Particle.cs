using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities.Particles
{
	abstract class Particle
	{
		static Particle()
		{
			Spritesheet = ContentLoader.LoadTexture("Particles");
		}

		protected static Texture2D Spritesheet { get; set; }

		private Vector2 velocity;

		public Particle(Vector2 position, Vector2 velocity)
		{
			this.velocity = velocity;

			Position = position;
		}

		protected Vector2 Position { get; private set; }

		public bool Destroyed { get; private set; }

		public virtual void Destroy()
		{
			Destroyed = true;
		}

		public virtual void Update(float dt)
		{
			Position += velocity * dt;
		}

		public abstract void Draw(SpriteBatch sb);
	}
}
