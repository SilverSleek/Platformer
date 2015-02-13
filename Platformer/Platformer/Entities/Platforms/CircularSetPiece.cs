using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities.Platforms
{
	class CircularSetPiece : SetPiece
	{
		private const int RADIUS_INCREMENT = 40;
		private const int PLATFORM_SPEED = 100;

		private Vector2 center;

		private int radius;
		private float baseAngle;
		private float angleIncrement;
		private float angularVelocity;

		public CircularSetPiece(Vector2 center, int numPlatforms) :
			base(numPlatforms)
		{
			this.center = center;

			radius = RADIUS_INCREMENT * numPlatforms;
			angleIncrement = MathHelper.TwoPi / numPlatforms;
			angularVelocity = (float)PLATFORM_SPEED / radius;
		}

		protected override void InitializePlatforms(int numPlatforms)
		{
			SetPlatformPositions();
		}

		public override void Update(float dt)
		{
			SetPlatformPositions();

			baseAngle += angularVelocity * dt;

			base.Update(dt);
		}

		private void SetPlatformPositions()
		{
			for (int i = 0; i < OriginalNumPlatforms; i++)
			{
				if (!Platforms[i].Destroyed)
				{
					Platforms[i].SetPosition(center + Functions.ComputePosition(baseAngle + angleIncrement * i, radius));
				}
			}
		}

		public override void Draw(SpriteBatch sb)
		{
		}
	}
}
