using Platformer.Entities.Events;

namespace Platformer.Interfaces
{
	interface IEventListener
	{
		void EventResponse(SimpleEvent simpleEvent);
	}
}
