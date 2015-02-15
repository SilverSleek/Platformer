using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Platforms;
using Platformer.Shared;

namespace Platformer.Entities.Hazards
{
	class StationarySpikes : Hazard
	{
		private static Texture2D texture;
		private static int textureWidth;

		static StationarySpikes()
		{
			texture = ContentLoader.LoadTexture("Hazards/SpikeDown");
			textureWidth = texture.Width;
		}

		private Vector2 position;
		private int numSpikes;

		public StationarySpikes(Platform platform) :
			base(HazardTypes.STATIONARY_SPIKES)
		{
			BoundingBox2D platformBox = platform.BoundingBox;

			position = new Vector2(platformBox.Left, platformBox.Bottom);
			numSpikes = (int)platformBox.Width / textureWidth;
			BoundingBox = new BoundingBox2D(new Vector2(platformBox.Center.X, platformBox.Bottom), platformBox.Width,
				platformBox.Height);
			Active = true;
		}

		public override void Draw(SpriteBatch sb)
		{
			Vector2 drawPosition = position;

			for (int i = 0; i < numSpikes; i++)
			{
				sb.Draw(texture, drawPosition, Color.White);

				drawPosition.X += textureWidth;
			}
		}
	}
}
