using Platformer.Interfaces;

namespace Platformer.Entities.Events
{
	class ListenerEventData
	{
		public ListenerEventData(EventTypes eventType, ISimpleEventListener listener)
		{
			EventType = eventType;
			Listener = listener;
		}

		public EventTypes EventType { get; private set; }
		public ISimpleEventListener Listener { get; private set; }
	}
}
