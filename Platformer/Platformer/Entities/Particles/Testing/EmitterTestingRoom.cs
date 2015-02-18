using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Events;
using Platformer.Entities.Particles.Emitters;
using Platformer.Interfaces;

namespace Platformer.Entities.Particles.Testing
{
	class EmitterTestingRoom : IEventListener
	{
		private const int LIST_SPACING = 200;

		private List<Emitter> emitters;

		private SelectionList emitterList;
		private SelectionList groupList;

		public EmitterTestingRoom()
		{
			emitterList = new SelectionList(typeof(EmitterTypes));
			groupList = new SelectionList(typeof(EmitterGroupTypes));

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.MOUSE, this));
		}

		public void Clear()
		{
			foreach (Emitter emitter in emitters)
			{
				emitter.Destroy();
			}

			emitters.Clear();
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			MouseEventData data = (MouseEventData)simpleEvent.Data;

			if (data.RightButtonState == ButtonStates.PRESSED_THIS_FRAME)
			{
				if (!emitterList.Active)
				{
					emitterList.Activate(Vector2.Zero);
					groupList.Activate(Vector2.Zero);
				}
				else
				{
					emitterList.Active = false;
					groupList.Active = false;
				}
			}

			if (emitterList.Active)
			{
				CheckSelection(emitterList, data);
			}
		}

		private void CheckSelection(SelectionList list, MouseEventData data)
		{
		}

		public void Update(float dt)
		{
			if (emitterList.Active)
			{
				emitterList.Update();
				groupList.Update();
			}
		}

		public void Draw(SpriteBatch sb)
		{
			// both lists will always be active at the same time
			if (emitterList.Active)
			{
				emitterList.Draw(sb);
				groupList.Draw(sb);
			}
		}
	}
}
