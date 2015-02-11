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
		private Hazard lastHazard;

		private List<Platform> platforms;
		private List<Hazard> hazards;

		private bool intersectingLava;

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

			CheckLava(playerBox);
			CheckPlatforms(playerBox);
			CheckHazards(playerBox);
		}

		private void CheckLava(Rectangle playerBox)
		{
			if (CheckLavaIntersection(playerBox))
			{
				if (!intersectingLava)
				{
					player.RegisterDamage(CollisionDirections.DOWN);
					intersectingLava = true;
				}
			}
			else if (intersectingLava)
			{
				intersectingLava = false;
			}
		}

		private bool CheckLavaIntersection(Rectangle playerBox)
		{
			Vector2[] points = lava.Points;

			int leftIndex = (int)(playerBox.Left / lava.Increment);
			int rightIndex = (int)(playerBox.Right / lava.Increment) + 1;

			for (int i = leftIndex; i <= rightIndex; i++)
			{
				if (playerBox.Contains((int)points[i].X, (int)points[i].Y))
				{
					return true;
				}
			}

			return false;
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
						player.RegisterPlatformCollision(platform, CollisionDirections.DOWN, platformBox.Top);
					}
				}
			}
		}

		private void CheckHazards(Rectangle playerBox)
		{
			foreach (Hazard hazard in hazards)
			{
				Rectangle hazardBox = hazard.BoundingBox;

				if (hazard == lastHazard)
				{
					if (!lastHazard.Active || Rectangle.Intersect(playerBox, hazardBox) == Rectangle.Empty)
					{
						lastHazard = null;
					}
				}
				else if (hazard.Active)
				{
					Rectangle intersection = Rectangle.Intersect(playerBox, hazardBox);

					if (intersection != Rectangle.Empty)
					{
						lastHazard = hazard;
						player.RegisterDamage(CollisionDirections.UP);
					}
				}
			}
		}
	}
}
