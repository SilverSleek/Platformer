using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities;

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

		public static void DrawBoundingBox(SpriteBatch sb, BoundingBox2D boundingBox, Color color)
		{
			int width = (int)boundingBox.Width;
			int height = (int)boundingBox.Height;
			int x = (int)boundingBox.Center.X - width / 2;
			int y = (int)boundingBox.Center.Y - height / 2;

			Rectangle destinationRectLeft = new Rectangle(x, y, 1, height);
			Rectangle destinationRectRight = new Rectangle(x + width, y, 1, height);
			Rectangle destinationRectTop = new Rectangle(x, y, width, 1);
			Rectangle destinationRectBottom = new Rectangle(x, y + height, width, 1);

			sb.Draw(whitePixel, destinationRectLeft, color);
			sb.Draw(whitePixel, destinationRectRight, color);
			sb.Draw(whitePixel, destinationRectTop, color);
			sb.Draw(whitePixel, destinationRectBottom, color);
		}

		public static void FillBoundingBox(SpriteBatch sb, BoundingBox2D boundingBox, Color color)
		{
			int width = (int)boundingBox.Width;
			int height = (int)boundingBox.Height;
			int x = (int)boundingBox.Center.X - width / 2;
			int y = (int)boundingBox.Center.Y - height / 2;

			sb.Draw(whitePixel, new Rectangle(x, y, width, height), color);
		}
	}
}
