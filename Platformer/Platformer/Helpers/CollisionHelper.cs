using Platformer.Entities;
using Platformer.Shared;

namespace Platformer.Helpers
{
	class CollisionHelper
	{
		private Player player;

		public CollisionHelper(Player player)
		{
			this.player = player;
		}

		public void Update()
		{
			if (player.BoundingBox.Bottom >= Constants.SCREEN_HEIGHT)
			{
				player.RegisterCollision(CardinalDirections.DOWN, Constants.SCREEN_HEIGHT);
			}
		}
	}
}
