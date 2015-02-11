using System;

using Microsoft.Xna.Framework;

namespace Platformer.Shared
{
	class Functions
	{
		public static float ComputeAngle(Vector2 start, Vector2 end)
		{
			float dX = end.X - start.X;
			float dY = end.Y - start.Y;

			return (float)Math.Atan2(dY, dX);
		}
	}
}
