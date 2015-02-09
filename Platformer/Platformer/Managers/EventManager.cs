using System.Collections.Generic;

using Platformer.Entities.Events;
using Platformer.Interfaces;

namespace Platformer.Managers
{
	class EventManager : IEventListener
	{
		private Dictionary<EventTypes, List<IEventListener>> listenerMap;

		public EventManager()
		{
			listenerMap = new Dictionary<EventTypes, List<IEventListener>>();
			listenerMap.Add(EventTypes.LISTENER, new List<IEventListener>());
			listenerMap[EventTypes.LISTENER].Add(this);
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			ListenerEventData data = (ListenerEventData)simpleEvent.Data;
			EventTypes eventType = data.EventType;

			if (!listenerMap.ContainsKey(eventType))
			{
				listenerMap.Add(eventType, new List<IEventListener>());
			}

			listenerMap[eventType].Add(data.Listener);
		}

		public void Update()
		{
			Queue<SimpleEvent> eventQueue = SimpleEvent.Queue;

			while (eventQueue.Count > 0)
			{
				SimpleEvent simpleEvent = eventQueue.Dequeue();

				if (listenerMap.ContainsKey(simpleEvent.Type))
				{
					foreach (IEventListener listener in listenerMap[simpleEvent.Type])
					{
						listener.EventResponse(simpleEvent);
					}
				}
			}
		}
	}
}
