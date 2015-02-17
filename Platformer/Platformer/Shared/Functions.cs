using System;

using Microsoft.Xna.Framework;

namespace Platformer.Shared
{
	class Functions
	{
		private static Random random;

		static Functions()
		{
			random = new Random();
		}

		public static int GetRandomInt(int min, int max)
		{
			return random.Next(min, max + 1);
		}

		public static float GetRandomFloat(float min, float max)
		{
			return (float)random.NextDouble() * (max - min) + min;
		}

		public static float ComputeAngle(Vector2 start, Vector2 end)
		{
			float dX = end.X - start.X;
			float dY = end.Y - start.Y;

			return (float)Math.Atan2(dY, dX);
		}

		public static Vector2 ComputePosition(float angle, float radius)
		{
			float x = (float)Math.Cos(angle);
			float y = (float)Math.Sin(angle);

			return new Vector2(x, y) * radius;
		}
	}
}
