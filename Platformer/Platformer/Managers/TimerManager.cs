using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Platformer.Entities;

namespace Platformer.Managers
{
	class TimerManager
	{
		private List<Timer> timers;

		public TimerManager()
		{
			timers = new List<Timer>();

			Timer.Initialize(timers);
		}

		public void Update(float dt)
		{
			int milliseconds = (int)(dt * 1000);

			for (int i = 0; i < timers.Count; i++)
			{
				Timer timer = timers[i];

				if (timer.Destroy)
				{
					timers.RemoveAt(i);
				}
				else if (!timer.Paused)
				{
					timer.Delay += milliseconds;

					if (timer.Delay >= timer.Duration)
					{
						timer.Trigger();

						if (timer.Repeating)
						{
							// this preserves leftover time
							timer.Delay = timer.Duration - timer.Delay;
						}
						else
						{
							timers.RemoveAt(i);
							timer.Destroy = true;
						}
					}

					if (!timer.Destroy)
					{
						timer.Progress = (float)timer.Delay / timer.Duration;
					}
				}
			}
		}
	}
}
