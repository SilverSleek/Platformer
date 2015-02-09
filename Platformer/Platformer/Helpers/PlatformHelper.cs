using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities;
using Platformer.Entities.Events;
using Platformer.Interfaces;
using Platformer.Shared;

namespace Platformer.Helpers
{
	class PlatformHelper : IEventListener
	{
		private const int VERTICAL_SPACING = 150;
		private const int OFFSCREEN_DISTANCE = 100;
		private const int GENERATION_EDGE_OFFSET = 100;

		private List<Platform> platforms;
		private Lava lava;
		private Random random;

		public PlatformHelper(List<Platform> platforms, Lava lava)
		{
			this.platforms = platforms;
			this.lava = lava;

			random = new Random();

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			platforms.Clear();
			platforms.Add(new Platform(new Vector2(400, 400)));
		}

		public void Update(float dt)
		{
			float lavaLevel = lava.Level;

			for (int i = 0; i < platforms.Count; i++)
			{
				if (platforms[i].BoundingBox.Top > lavaLevel)
				{
					platforms.RemoveAt(i);
				}
				else
				{
					platforms[i].Update(dt);
				}
			}

			GeneratePlatforms();
		}

		private void GeneratePlatforms()
		{
			int topY = platforms[platforms.Count - 1].BoundingBox.Y;

			if (topY > Camera.Instance.VisibleArea.Top + VERTICAL_SPACING - OFFSCREEN_DISTANCE)
			{
				int x = random.Next(GENERATION_EDGE_OFFSET, Constants.SCREEN_WIDTH - GENERATION_EDGE_OFFSET + 1);
				int y = topY - VERTICAL_SPACING;

				platforms.Add(new Platform(new Vector2(x, y)));
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Platform platform in platforms)
			{
				platform.Draw(sb);
			}
		}
	}
}
