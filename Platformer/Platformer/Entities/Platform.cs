﻿using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Hazards;
using Platformer.Shared;

namespace Platformer.Entities
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

		public Platform(Vector2 position)
		{
			Texture2D texture = ContentLoader.LoadTexture("Platform");

			sprite = new Sprite(texture, position);
			BoundingBox = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2, texture.Width,
				texture.Height);
			hazard = new StationarySpikes(this);

			hazards.Add(hazard);
		}

		public Rectangle BoundingBox { get; private set; }

		public void Update(float dt)
		{
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
