using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Entities.Hazards
{
	public enum HazardTypes
	{
		STATIONARY_SPIKES,
		RETRACTABLE_SPIKES,
		LAVA_FALLS_SOURCE,
		LAVA_FALLS_BASE,
		NONE
	}

	abstract class Hazard
	{
		public Rectangle BoundingBox { get; protected set; }
		public bool Active { get; protected set; }

		public virtual void Destroy()
		{
		}

		public virtual void Update(float dt)
		{
		}

		public abstract void Draw(SpriteBatch sb);
	}
}
