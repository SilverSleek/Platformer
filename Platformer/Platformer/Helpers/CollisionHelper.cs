using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Platformer.Entities;
using Platformer.Entities.Events;
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

			CheckLava(playerBox);
			CheckPlatforms(playerBox);
		}

		private void CheckLava(Rectangle playerBox)
		{
			if (playerBox.Bottom >= lava.Level)
			{
				SimpleEvent.Queue.Enqueue(new SimpleEvent(EventTypes.RESET, null));
			}
		}

		private void CheckPlatforms(Rectangle playerBox)
		{
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
