using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities
{
	class Background
	{
		private Texture2D texture;

		private int tileSize;
		private int numTilesVertical;
		private int numTilesHorizontal;

		public Background()
		{
			texture = ContentLoader.LoadTexture("Background");
			tileSize = texture.Width;
			numTilesHorizontal = Constants.SCREEN_WIDTH / tileSize + 2;
			numTilesVertical = Constants.SCREEN_HEIGHT / tileSize + 2; 
		}

		public void Draw(SpriteBatch sb)
		{
			Vector2 startPosition = ComputeStartPosition();

			for (int i = 0; i < numTilesVertical; i++)
			{
				for (int j = 0; j < numTilesHorizontal; j++)
				{
					Vector2 drawPosition = new Vector2(j * tileSize, i * tileSize);

					sb.Draw(texture, startPosition + drawPosition, Color.White);
				}
			}
		}

		private Vector2 ComputeStartPosition()
		{
			Rectangle visibleArea = Camera.Instance.VisibleArea;

			int x = (int)(visibleArea.X / tileSize);
			int y = (int)(visibleArea.Y / tileSize);

			if (visibleArea.X < 0)
			{
				x--;
			}

			if (visibleArea.Y < 0)
			{
				y--;
			}

			return new Vector2(x * tileSize, y * tileSize);
		}
	}
}
