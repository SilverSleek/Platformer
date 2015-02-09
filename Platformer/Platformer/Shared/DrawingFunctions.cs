using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Shared
{
	class DrawingFunctions
	{
		private static Texture2D whitePixel;

		static DrawingFunctions()
		{
			whitePixel = ContentLoader.LoadTexture("WhitePixel");
		}

		public static void DrawRectangle(SpriteBatch sb, Rectangle rectangle, Color color)
		{
			int x = rectangle.X;
			int y = rectangle.Y;
			int width = rectangle.Width;
			int height = rectangle.Height;

			Rectangle destinationRectLeft = new Rectangle(x, y, 1, height);
			Rectangle destinationRectRight = new Rectangle(x + width, y, 1, height);
			Rectangle destinationRectTop = new Rectangle(x, y, width, 1);
			Rectangle destinationRectBottom = new Rectangle(x, y + height, width, 1);

			sb.Draw(whitePixel, destinationRectLeft, color);
			sb.Draw(whitePixel, destinationRectRight, color);
			sb.Draw(whitePixel, destinationRectTop, color);
			sb.Draw(whitePixel, destinationRectBottom, color);
		}
	}
}
