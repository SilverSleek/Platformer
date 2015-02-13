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

			InitializePlatforms();
		}

		protected override void InitializePlatforms()
		{
			SetPlatformPositions();
		}

		public override void Update(float dt)
		{
			SetPlatformPositions();

			baseAngle += angularVelocity * dt;

			base.Update(dt);
		}

		protected void SetPlatformPositions()
		{
			// using the original number of platforms preserves spacing of platforms around the circle even if one is destroyed
			for (int i = 0; i < OriginalNumPlatforms; i++)
			{
				if (!Platforms[i].Destroyed)
				{
					Platforms[i].SetCenter(center + Functions.ComputePosition(baseAngle + angleIncrement * i, radius));
				}
			}
		}

		public override void Draw(SpriteBatch sb)
		{
		}
	}
}
