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
			//Rectangle platformBox = platform.BoundingBox;

			//position = new Vector2(platformBox.Left, platformBox.Bottom);
			//numSpikes = platformBox.Width / textureWidth;
			//BoundingBox = new Rectangle((int)position.X, (int)position.Y, numSpikes * textureWidth, texture.Height);
			//Active = true;
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
