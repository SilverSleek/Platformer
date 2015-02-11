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

		private bool submerged;

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
			Vector2 bottomLeft = new Vector2(playerBox.Left, playerBox.Bottom);
			Vector2 bottomRight = new Vector2(playerBox.Right, playerBox.Bottom);

			if (lava.CheckSubmerged(bottomLeft) || lava.CheckSubmerged(bottomRight))
			{
				if (!submerged)
				{
					player.RegisterDamage(CollisionDirections.DOWN);
					submerged = true;
				}
			}
			else if (submerged)
			{
				submerged = false;
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
