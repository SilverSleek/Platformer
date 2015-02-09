using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities;
using Platformer.Entities.Events;
using Platformer.Interfaces;

namespace Platformer.Helpers
{
	class PlatformHelper : IEventListener
	{
		private List<Platform> platforms;
		private Lava lava;

		public PlatformHelper(List<Platform> platforms, Lava lava)
		{
			this.platforms = platforms;
			this.lava = lava;

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			platforms.Clear();
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
