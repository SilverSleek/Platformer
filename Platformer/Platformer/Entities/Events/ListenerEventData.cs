using Platformer.Interfaces;

namespace Platformer.Entities.Events
{
	class ListenerEventData
	{
		public ListenerEventData(EventTypes eventType, IEventListener listener)
		{
			EventType = eventType;
			Listener = listener;
		}

		public EventTypes EventType { get; private set; }
		public IEventListener Listener { get; private set; }
	}
}
