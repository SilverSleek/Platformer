using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities.Particles
{
	class Ash : Particle
	{
		private const float MIN_ANGULAR_VELOCITY = -1.5f;
		private const float MAX_ANGULAR_VELOCITY = 1.5f;
		private const int MIN_SCALE_DELAY = 750;
		private const int MAX_SCALE_DELAY = 1250;
		private const int MIN_SCALE_DURATION = 600;
		private const int MAX_SCALE_DURATION = 800;
        private const int MIN_LIFETIME = 5000;
        private const int MAX_LIFETIME = 7500;

		private enum ScaleStates
		{
			SHRINK,
			EXPAND,
			NONE
		}

		private Timer timer;
		private ScaleStates scaleState;

		public Ash(Vector2 position, Vector2 velocity) :
			base(ParticleTypes.ASH, position, velocity, MIN_ANGULAR_VELOCITY, MAX_ANGULAR_VELOCITY, MIN_LIFETIME,
            MAX_LIFETIME)
		{
			ChangeScaleState(1, ScaleStates.NONE, MIN_SCALE_DELAY, MAX_SCALE_DELAY, BeginShrink);
		}

		private void BeginShrink()
		{
			ChangeScaleState(1, ScaleStates.SHRINK, MIN_SCALE_DURATION, MAX_SCALE_DURATION, BeginExpand);
		}

		private void BeginExpand()
		{
			ChangeScaleState(0, ScaleStates.EXPAND, MIN_SCALE_DURATION, MAX_SCALE_DURATION, EndExpand);
		}

		private void EndExpand()
		{
			ChangeScaleState(1, ScaleStates.NONE, MIN_SCALE_DELAY, MAX_SCALE_DELAY, BeginShrink);
		}

		private void ChangeScaleState(float scaleValue, ScaleStates scaleState, int minDuration, int maxDuration, Action trigger)
		{
			this.scaleState = scaleState;

			int delay = (int)Functions.GetRandomValue(minDuration, maxDuration);

            Vector2 scale = Scale;
            scale.X = scaleValue;
			Scale = scale;

			timer = new Timer(delay, trigger, false);
		}

		public override void Destroy()
		{
			timer.Destroy = true;

			base.Destroy();
		}

		public override void Update(float dt)
		{
            Vector2 scale = Scale;

			if (scaleState == ScaleStates.SHRINK)
			{
				scale.X = MathHelper.Lerp(1, 0, timer.Progress);
			}
			else if (scaleState == ScaleStates.EXPAND)
			{
				scale.X = MathHelper.Lerp(0, 1, timer.Progress);
			}

            Scale = scale;

			base.Update(dt);
		}
	}
}
