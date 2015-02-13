using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Events;
using Platformer.Interfaces;
using Platformer.Shared;

namespace Platformer.Entities
{
	class HeightDisplay : IEventListener
	{
		private const int VERTICAL_OFFSET = 25;

		private Text text;
		private int maxHeightReached;

		public HeightDisplay()
		{
			text = new Text(ContentLoader.LoadFont("Height"), "0", new Vector2(Constants.SCREEN_WIDTH / 2, VERTICAL_OFFSET), Color.Black);
			maxHeightReached = -1;

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		public void SetValue(int heightValue)
		{
			if (maxHeightReached == -1 || heightValue > maxHeightReached)
			{
				maxHeightReached = heightValue;
				text.SetValue(heightValue.ToString());
			}
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			maxHeightReached = -1;
			text.SetValue("0");
		}

		public void Draw(SpriteBatch sb)
		{
			sb.End();
			sb.Begin();

			text.Draw(sb);
		}
	}
}
