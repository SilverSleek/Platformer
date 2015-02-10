using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Platformer.Entities;
using Platformer.Entities.Events;
using Platformer.Entities.Hazards;
using Platformer.Interfaces;
using Platformer.Shared;

namespace Platformer.Helpers
{
	class CollisionHelper : IEventListener
	{
		private Player player;
		private Lava lava;
		private List<Platform> platforms;
		private List<Hazard> hazards;

		public CollisionHelper(Player player, Lava lava, List<Platform> platforms, List<Hazard> hazards)
		{
			this.player = player;
			this.lava = lava;
			this.platforms = platforms;
			this.hazards = hazards;

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			hazards.Clear();
			platforms.Clear();
			platforms.Add(new Platform(new Vector2(400, 400), HazardTypes.NONE));
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
			CheckHazards(playerBox);
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

		private void CheckHazards(Rectangle playerBox)
		{
			foreach (Hazard hazard in hazards)
			{
				if (hazard.Active)
				{
					Rectangle hazardBox = hazard.BoundingBox;
					Rectangle intersection = Rectangle.Intersect(playerBox, hazardBox);

					if (intersection != Rectangle.Empty)
					{
						SimpleEvent.AddEvent(EventTypes.RESET, null);
					}
				}
			}
		}
	}
}
