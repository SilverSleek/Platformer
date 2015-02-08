using Platformer.Entities.Events;

namespace Platformer.Interfaces
{
	interface ISimpleEventListener
	{
		void EventResponse(SimpleEvent simpleEvent);
	}
}
