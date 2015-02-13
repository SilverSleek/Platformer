using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Platformer.Entities;
using Platformer.Entities.Events;
using Platformer.Entities.Hazards;
using Platformer.Entities.Platforms;
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
		}

		public void Update()
		{
			BoundingBox2D playerBox = player.NewBoundingBox;

			CheckLava(playerBox);
			CheckPlatforms(playerBox);
			CheckHazards(playerBox);
		}

		private void CheckLava(BoundingBox2D playerBox)
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

		private void CheckPlatforms(BoundingBox2D playerBox)
		{
			foreach (Platform platform in platforms)
			{
				BoundingBox2D platformBox = platform.BoundingBox;

				if (playerBox.Intersects(platformBox))
				{
					player.RegisterPlatformCollision(platform);
				}
			}
		}

		private void CheckHazards(BoundingBox2D playerBox)
		{
			foreach (Hazard hazard in hazards)
			{
				BoundingBox2D hazardBox = hazard.BoundingBox;

				if (hazard == lastHazard)
				{
					if (!lastHazard.Active || !playerBox.Intersects(hazardBox))
					{
						lastHazard = null;
					}
				}
				else if (hazard.Active)
				{
					if (playerBox.Intersects(hazardBox))
					{
						lastHazard = hazard;
						player.RegisterDamage(GetCollisionDirection(playerBox, hazardBox, hazard.Type));
					}
				}
			}
		}

		private CollisionDirections GetCollisionDirection(BoundingBox2D playerBox, BoundingBox2D hazardBox, HazardTypes hazardType)
		{
			switch (hazardType)
			{
				case HazardTypes.STATIONARY_SPIKES:
					return CollisionDirections.UP;
					
				case HazardTypes.RETRACTABLE_SPIKES:
					return CollisionDirections.DOWN;

				case HazardTypes.LAVA_FALLS:
					return playerBox.Left < hazardBox.Left ? CollisionDirections.RIGHT : CollisionDirections.LEFT;
			}

			return CollisionDirections.NONE;
		}
	}
}
