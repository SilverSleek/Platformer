using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities;
using Platformer.Entities.Events;
using Platformer.Entities.Hazards;
using Platformer.Entities.Platforms;
using Platformer.Interfaces;
using Platformer.Shared;

namespace Platformer.Helpers
{
	class PlatformHelper : IEventListener
	{
		private const int MIN_VERTICAL_SPACING = 75;
		private const int MAX_VERTICAL_SPACING = 175;
		private const int GENERATION_VERTICAL_OFFSET = 20;
		private const int GENERATION_EDGE_OFFSET = 75;

		private Lava lava;

		private List<Platform> platforms;
		private List<SetPiece> setPieces;

		public PlatformHelper(List<Platform> platforms, Lava lava)
		{
			this.platforms = platforms;
			this.lava = lava;

			setPieces = new List<SetPiece>();

			SetPiece.Initialize(platforms);

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			platforms.Clear();
			setPieces.Clear();

			platforms.Add(new Platform(new Vector2(400, 400), HazardTypes.NONE, false));
		}

		public void Update(float dt)
		{
			for (int i = 0; i < setPieces.Count; i++)
			{
				SetPiece setPiece = setPieces[i];

				if (setPiece.Destroyed)
				{
					setPieces.RemoveAt(i);
				}
				else
				{
					setPiece.Update(dt);
				}
			}

			for (int i = 0; i < platforms.Count; i++)
			{
				Platform platform = platforms[i];

				if (platform.Destroyed)
				{
					platforms.RemoveAt(i);
				}
				else
				{
					BoundingBox2D boundingBox = platform.BoundingBox;
					Vector2 bottomLeft = new Vector2(boundingBox.Left, boundingBox.Bottom);
					Vector2 bottomRight = new Vector2(boundingBox.Right, boundingBox.Bottom);

					if (lava.CheckSubmerged(bottomLeft) || lava.CheckSubmerged(bottomRight))
					{
						platform.Destroy();
						platforms.RemoveAt(i);
					}
					else
					{
						platform.Update(dt);
					}
				}
			}

			GeneratePlatforms();
		}

		private void GeneratePlatforms()
		{
			float topY = platforms[platforms.Count - 1].BoundingBox.Center.Y;

			if (topY > Camera.Instance.VisibleArea.Top + MIN_VERTICAL_SPACING)
			{
				float x = Functions.GetRandomValue(GENERATION_EDGE_OFFSET, Constants.SCREEN_WIDTH - GENERATION_EDGE_OFFSET + 1);
				float y = topY - Functions.GetRandomValue(MIN_VERTICAL_SPACING, MAX_VERTICAL_SPACING) - GENERATION_VERTICAL_OFFSET;

				platforms.Add(new Platform(new Vector2(x, y), HazardTypes.NONE, false));
			}
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Platform platform in platforms)
			{
				platform.Draw(sb);
			}
		}
	}
}
