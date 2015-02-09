using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities
{
	class Platform
	{
		private Sprite sprite;

		public Platform(Vector2 position)
		{
			Texture2D texture = ContentLoader.LoadTexture("Platform");

			sprite = new Sprite(texture, position);
			BoundingBox = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2, texture.Width,
				texture.Height);
		}

		public Rectangle BoundingBox { get; private set; }

		public void Draw(SpriteBatch sb)
		{
			sprite.Draw(sb);
		}
	}
}
