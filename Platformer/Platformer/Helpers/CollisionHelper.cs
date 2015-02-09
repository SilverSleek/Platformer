using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Platformer.Entities;
using Platformer.Shared;

namespace Platformer.Helpers
{
	class CollisionHelper
	{
		private Player player;
		private Lava lava;
		private List<Platform> platforms;

		public CollisionHelper(Player player, Lava lava, List<Platform> platforms)
		{
			this.player = player;
			this.lava = lava;
			this.platforms = platforms;
		}

		public void Update()
		{
			Rectangle playerBox = player.NewBoundingBox;

			if (playerBox.Bottom >= Constants.SCREEN_HEIGHT)
			{
				player.RegisterCollision(CollisionDirections.DOWN, Constants.SCREEN_HEIGHT);
			}

			foreach (Platform platform in platforms)
			{
				Rectangle platformBox = platform.BoundingBox;
				Rectangle intersection = Rectangle.Intersect(playerBox, platformBox);

				if (intersection != Rectangle.Empty)
				{
					if (intersection.Width == playerBox.Width && player.OldBoundingBox.Bottom <= platformBox.Top)
					{
						player.RegisterCollision(CollisionDirections.DOWN, platformBox.Top);
					}
				}
			}
		}
	}
}
