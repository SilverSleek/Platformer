using System;

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

		public static void DrawPoint(SpriteBatch sb, Vector2 point, Color color)
		{
			sb.Draw(whitePixel, point, color);
		}

		public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color)
		{
			float length = (end - start).Length();
			float rotation = Functions.ComputeAngle(start, end);

			Rectangle sourceRect = new Rectangle(0, 0, (int)length, 1);

			sb.Draw(whitePixel, start, sourceRect, color, rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
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

		public static void FillRectangle(SpriteBatch sb, Rectangle rectangle, Color color)
		{
			sb.Draw(whitePixel, rectangle, color);
		}
	}
}
