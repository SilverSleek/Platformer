using System;

using Microsoft.Xna.Framework;

namespace Platformer.Entities
{
	class BoundingBox2D
	{
		public BoundingBox2D(BoundingBox2D other) :
			this(other.Center, other.Width, other.Height)
		{
		}

		public BoundingBox2D(Vector2 center, float width, float height)
		{
			Width = width;
			Height = height;

			SetCenter(center);
		}

		public Vector2 Center { get; private set; }

		public float Width { get; private set; }
		public float Height { get; private set; }
		public float Left { get; private set; }
		public float Right { get; private set; }
		public float Top { get; private set; }
		public float Bottom { get; private set; }

		public void SetCenter(Vector2 center)
		{
			Center = center;
			Left = Center.X - Width / 2;
			Right = Center.X + Width / 2;
			Top = Center.Y - Height / 2;
			Bottom = Center.Y + Height / 2;
		}

		public bool Contains(Vector2 point)
		{
			float dX = Math.Abs(Center.X - point.X);
			float dY = Math.Abs(Center.Y - point.Y);

			return dX <= Width / 2 && dY <= Height / 2;
		}

		public bool Intersects(BoundingBox2D other)
		{
			float dX = Math.Abs(Center.X - other.Center.X);
			float dY = Math.Abs(Center.Y - other.Center.Y);

			return dX <= (Width + other.Width) / 2 && dY <= (Height + other.Height) / 2;
		}
	}
}
