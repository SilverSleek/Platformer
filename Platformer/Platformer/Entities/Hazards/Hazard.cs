using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Entities.Hazards
{
	public enum HazardTypes
	{
		STATIONARY_SPIKES,
		RETRACTABLE_SPIKES,
		LAVA_FALLS,
		NONE
	}

	abstract class Hazard
	{
		static Hazard()
		{
			NumHazardTypes = Enum.GetNames(typeof(HazardTypes)).Length;
		}

		public static int NumHazardTypes { get; private set; }

		protected Hazard(HazardTypes type)
		{
			Type = type;
		}

		public BoundingBox2D BoundingBox { get; protected set; }
		public HazardTypes Type { get; private set; }
		
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
