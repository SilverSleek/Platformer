using System.Collections.Generic;

using Platformer.Interfaces;

namespace Platformer.Entities.Events
{
	public enum EventTypes
	{
		LISTENER,
		KEYBOARD,
		MOUSE,
		GAMESTATE,
		EXIT
	}

	class SimpleEvent
	{
		static SimpleEvent()
		{
			Queue = new Queue<SimpleEvent>();
		}

		public static Queue<SimpleEvent> Queue { get; private set; }

		public static void AddListener(EventTypes eventType, ISimpleEventListener listener)
		{
			Queue.Enqueue(new SimpleEvent(EventTypes.LISTENER, new ListenerEventData(eventType, listener)));
		}

		public SimpleEvent(EventTypes type, object data)
		{
			Type = type;
			Data = data;
		}

		public EventTypes Type { get; private set; }

		public object Data { get; private set; }
	}
}
