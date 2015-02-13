using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Hazards;
using Platformer.Shared;

namespace Platformer.Entities.Platforms
{
	class Platform
	{
		private static List<Hazard> hazards;

		public static void Initialize(List<Hazard> hazards)
		{
			Platform.hazards = hazards;
		}

		private Sprite sprite;
		private Hazard hazard;

		// this constructor is called from set pieces, so the platform is assumed to be moving
		public Platform() :
			this(Vector2.Zero, HazardTypes.NONE)
		{
			Moving = true;
		}

		public Platform(Vector2 position, HazardTypes hazardType)
		{
			Texture2D texture = ContentLoader.LoadTexture("Platform");

			sprite = new Sprite(texture, position);
			BoundingBox = new BoundingBox2D(position, texture.Width, texture.Height);

			if (hazardType != HazardTypes.NONE)
			{
				CreateHazard(hazardType);
			}
		}

		public Vector2 Center { get; private set; }
		public BoundingBox2D BoundingBox { get; private set; }

		public bool Moving { get; private set; }
		public bool Destroyed { get; set; }

		private void CreateHazard(HazardTypes hazardType)
		{
			switch (hazardType)
			{
				case HazardTypes.STATIONARY_SPIKES:
					hazard = new StationarySpikes(this);
					break;

				case HazardTypes.RETRACTABLE_SPIKES:
					hazard = new RetractableSpikes(this);
					break;

				case HazardTypes.LAVA_FALLS:
					hazard = new Lavafall(this);
					break;
			}

			hazards.Add(hazard);
		}

		public void Destroy()
		{
			Destroyed = true;

			if (hazard != null)
			{
				hazard.Destroy();
			}
		}

		public void SetPosition(Vector2 position)
		{
			Center = position;
			sprite.Position = position;
			BoundingBox.SetCenter(position);
		}

		public void Update(float dt)
		{
			if (hazard != null)
			{
				hazard.Update(dt);
			}
		}

		public void Draw(SpriteBatch sb)
		{
			if (hazard != null)
			{
				hazard.Draw(sb);
			}

			sprite.Draw(sb);
		}
	}
}
