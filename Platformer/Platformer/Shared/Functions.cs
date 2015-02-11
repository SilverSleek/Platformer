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

		public static float GetRandomValue(float min, float max)
		{
			return (float)random.NextDouble() * (max - min) + min;
		}

		public static float ComputeAngle(Vector2 start, Vector2 end)
		{
			float dX = end.X - start.X;
			float dY = end.Y - start.Y;

			return (float)Math.Atan2(dY, dX);
		}
	}
}
