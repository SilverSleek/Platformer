using Microsoft.Xna.Framework;

using Platformer.Factories;
using Platformer.Shared;

namespace Platformer.Entities.Particles.Emitters
{
	class ContinuousEmitter : Emitter
	{
		private Timer timer;

		public ContinuousEmitter(EmitterTypes type, Vector2 position) :
			base(type)
		{
			Position = position;

			// setting the duration to zero makes the timer trigger immediately, and its duration is changed every emit anyway
			timer = new Timer(0, Emit, true);
		}

		public override void Destroy()
		{
			timer.Destroy = true;
		}

		public override void Emit()
		{
			int numParticles = Functions.GetRandomInt(Attributes.MinParticles, Attributes.MaxParticles);

			for (int i = 0; i < numParticles; i++)
			{
				float speedX = Functions.GetRandomFloat(Attributes.MinParticleSpeedX, Attributes.MaxParticleSpeedX);
				float speedY = Functions.GetRandomFloat(Attributes.MinParticleSpeedY, Attributes.MaxParticleSpeedY);

				ParticleFactory.CreateParticle(Attributes.ParticleType, Position, new Vector2(speedX, speedY));

				timer.Duration = Functions.GetRandomInt(Attributes.MinDelay, Attributes.MaxDelay);
			}
		}

		public Vector2 Position { get; set; }
	}
}
