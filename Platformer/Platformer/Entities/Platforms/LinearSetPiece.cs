using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities.Platforms
{
	public enum MovementDirections
	{
		LEFT,
		RIGHT
	}

	class LinearSetPiece : SetPiece
	{
		private const int OFFSCREEN_DISTANCE = 100;
		private const int PLATFORM_SPEED = 100;

		private static int totalLength;

		static LinearSetPiece()
		{
			totalLength = Constants.SCREEN_WIDTH + OFFSCREEN_DISTANCE * 2;
		}

		private MovementDirections movementDirection;

		private int yValue;

		private Vector2 platformVelocity;

		public LinearSetPiece(int yValue, int numPlatforms, MovementDirections movementDirection) :
			base(numPlatforms)
		{
			this.yValue = yValue;
			this.movementDirection = movementDirection;

			InitializePlatforms();
		}

		protected override void InitializePlatforms()
		{
			float startingOffset = (float)totalLength / Platforms.Count / 2;

			Vector2 offset = new Vector2((float)totalLength / Platforms.Count, 0);
			Vector2 startingPosition = Vector2.Zero;

			offset *= movementDirection == MovementDirections.RIGHT ? -1 : 1;

			if (movementDirection == MovementDirections.LEFT)
			{
				platformVelocity = new Vector2(-PLATFORM_SPEED, 0);
				startingPosition = new Vector2(startingOffset - OFFSCREEN_DISTANCE, yValue);
			}
			else
			{
				platformVelocity = new Vector2(PLATFORM_SPEED, 0);
				startingPosition = new Vector2(Constants.SCREEN_WIDTH + OFFSCREEN_DISTANCE - startingOffset, yValue);
			}

			for (int i = 0; i < Platforms.Count; i++)
			{
				Platforms[i].SetCenter(startingPosition + offset * i);
			}
		}

		private void CheckRecyclePlatform()
		{
			float x = Platforms[0].BoundingBox.Center.X;
			float newX = float.MinValue;

			Platform newPlatform = null;

			if (movementDirection == MovementDirections.LEFT)
			{
				if (x <= -OFFSCREEN_DISTANCE)
				{
					newX = x + totalLength;
				}
			}
			else
			{
				if (x >= Constants.SCREEN_WIDTH + OFFSCREEN_DISTANCE)
				{
					newX = x - totalLength;
				}
			}

			if (newX != float.MinValue)
			{
				newPlatform = new Platform(new Vector2(newX, yValue), Hazards.HazardTypes.NONE, true);

				Platforms[0].Destroy();
				Platforms.RemoveAt(0);
				Platforms.Add(newPlatform);
				PlatformMasterList.Add(newPlatform);
			}
		}

		public override void Update(float dt)
		{
			CheckRecyclePlatform();

			for (int i = 0; i < Platforms.Count; i++)
			{
				Platforms[i].SetCenter(Platforms[i].BoundingBox.Center + platformVelocity * dt);
			}

			base.Update(dt);
		}

		public override void Draw(SpriteBatch sb)
		{
		}
	}
}
