using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Platforms;
using Platformer.Shared;

namespace Platformer.Entities.Hazards
{
	class Lavafall : Hazard
	{
		private Vector2 bottomPosition;
		private Vector2 bottomVelocity;

		public Lavafall(Platform platform) :
			base(HazardTypes.LAVA_FALLS)
		{
			Rectangle platformBox = platform.BoundingBox;

			bottomPosition = new Vector2(platformBox.Left + platformBox.Width / 2, platformBox.Bottom);
			BoundingBox = new Rectangle(platformBox.Left, platformBox.Bottom, platformBox.Width, 0);
			Active = true;
		}

		public override void Update(float dt)
		{
			bottomVelocity.Y += Constants.GRAVITY * dt;
			bottomPosition += bottomVelocity * dt;

			Rectangle boundingBox = BoundingBox;
			boundingBox.Height = (int)bottomPosition.Y - boundingBox.Top;
			BoundingBox = boundingBox;
		}

		public override void Draw(SpriteBatch sb)
		{
			DrawingFunctions.FillRectangle(sb, BoundingBox, Color.Red);
		}
	}
}
