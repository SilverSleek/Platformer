using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Events;
using Platformer.Interfaces;
using Platformer.Shared;

namespace Platformer.Entities
{
	class Lava : IEventListener
	{
		private const int ASCENSION_SPEED = 50;

		private Texture2D whitePixel;

		public Lava()
		{
			whitePixel = ContentLoader.LoadTexture("WhitePixel");
			Level = Constants.SCREEN_HEIGHT;

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}
		
		public float Level { get; private set; }

		public void EventResponse(SimpleEvent simpleEvent)
		{
			Level = Constants.SCREEN_HEIGHT;
		}

		public void Update(float dt)
		{
			Level -= ASCENSION_SPEED * dt;
		}

		public void Draw(SpriteBatch sb)
		{
			Rectangle destinationRect = new Rectangle(0, (int)Level, Constants.SCREEN_WIDTH, 1);

			sb.Draw(whitePixel, destinationRect, Color.Red);
		}
	}
}
