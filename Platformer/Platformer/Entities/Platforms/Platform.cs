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

		public Platform() :
			this(Vector2.Zero, HazardTypes.NONE)
		{
		}

		public Platform(Vector2 position, HazardTypes hazardType)
		{
			Texture2D texture = ContentLoader.LoadTexture("Platform");

			sprite = new Sprite(texture, position);
			BoundingBox = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2, texture.Width,
				texture.Height);

			if (hazardType != HazardTypes.NONE)
			{
				CreateHazard(hazardType);
			}
		}

		public Rectangle BoundingBox { get; private set; }

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
			sprite.Position = position;

			Rectangle boundingBox = BoundingBox;
			boundingBox.X = (int)position.X - boundingBox.Width / 2;
			boundingBox.Y = (int)position.Y - boundingBox.Height / 2;
			BoundingBox = boundingBox;
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
